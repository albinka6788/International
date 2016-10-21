using AutoMapper;
using International.BusinessEntities;
using International.BusinessEntities.Models;
using International.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using International.BusinessLogic.Classes;

namespace International.BusinessLogic.Managers
{
    public class UserManager : BaseManager
    {

        public UserManager(InternationalSubmissionEntities applicationContext, International.Entities.MDMEntities.MDMEntities mdmContext) : base(applicationContext, mdmContext) { }

        #region Login Section

        public string SetCookie(UserModel user)
        {
            var loggedInUser = new LoggedInUser
            {
                ApplicationKey = ApplicationConstants.ApplicationKey,
                UserId = user.UserId,
                UserName = user.FirstName + " " + user.LastName,
                UserEmail = user.Email,
                UserMenu = user.UserMenu,
                UserRights = user.UserRights,
                UserModules = user.UserModules,
                IsAuthenticated = true
            };
            var ticket = new FormsAuthenticationTicket(1, user.UserId.ToString(), DateTime.Now, DateTime.Now.AddDays(30), false, new JavaScriptSerializer().Serialize(loggedInUser));
            // var userToken = FormsAuthentication.Encrypt(ticket);

            return ticket.UserData;
        }

        /// <summary>
        /// Method to validate user
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public UserModel ValidateUser(UserModel userModel)
        {
            var userDetails = (from tableData in Context.Users
                               where tableData.Email == userModel.Email && tableData.Password == userModel.Password
                               select tableData).FirstOrDefault();
            return (Mapper.DynamicMap<User, UserModel>(userDetails));
        }


        public string GetSession(string cookie)
        {
            var currentSession = GetGUID(cookie);
            var userDetails = (from tableData in Context.UserSessionLogs
                               where tableData.Id == currentSession && tableData.SessionStatus == true
                               select tableData).FirstOrDefault();
            return userDetails.SessionString ?? string.Empty;
        }



        public Guid GetGUID(string cookie)
        {
            Guid guid = Guid.Empty;
            if (!Guid.TryParse(cookie, out guid))
            {
                guid = Guid.Empty;
            }

            return guid;
        }

        /// <summary>
        /// Method to add cookie
        /// </summary>
        /// <param name="CountryMaster"></param>
        public string SaveCoikies(UserModel userModel, string token, string browser, string platform)
        {
            UserSessionLogModel logModel = new UserSessionLogModel();
            var uniqueId = Guid.NewGuid();
            logModel.Id = uniqueId;
            logModel.SessionString = token;
            logModel.UserId = userModel.UserId;
            logModel.SessionStatus = true;
            logModel.SessionBrowser = browser;
            logModel.SessionMedia = platform;
            logModel.SessionIP = GetLocalIPAddress();
            logModel.CreatedOnDate = DateTime.UtcNow;
            Context.UserSessionLogs.Add(Mapper.DynamicMap<UserSessionLog>(logModel));
            Context.SaveChanges();
            return uniqueId.ToString();
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return string.Empty;
        }





        #endregion

        #region User Section
        public UserModel GetUser(int userId)
        {
            var user = (from users in Context.Users
                        join groups in Context.Groups on users.GroupId equals groups.Id
                        where users.UserId == userId
                        select new UserModel
                        {
                            UserId = users.UserId,
                            FirstName = users.FirstName,
                            LastName = users.LastName,
                            Email = users.Email,
                            GroupNo = users.GroupId.ToString(),
                            GroupName = groups.GroupName,
                            Password = users.Password,
                            ConfirmPassword = users.Password,
                            CreatedByUserId = users.CreatedByUserId,
                            CreatedOnDate = users.CreatedOnDate,
                            IsActive = users.IsActive
                        }).FirstOrDefault() ?? new UserModel();


            user.RegionList = ApplicationContext.Master.GetRegions(null);
            user.UserRegionList = GetUserRegions(userId);
            user.GroupList = GetGroupList(null);
            return user;

        }


        public List<UserRegionModel> GetUserRegions(int userId)
        {
            var RegionList = ApplicationContext.Master.GetRegions(null);
            var userRegions = (from users in Context.UserRegions
                               where users.UserId == userId
                               select new UserRegionModel
                               {
                                   UserId = users.UserId,
                                   RegionId = users.RegionId,
                               }).ToList() ?? new List<UserRegionModel>();

            return (from region in RegionList
                    join uRegion in userRegions on region.id equals uRegion.RegionId
                    select new UserRegionModel()
                    {
                        label = region.label,
                        id = (int)region.id,
                    }).ToList();
        }

        public List<SelectListItem> GetGroupList(int? id)
        {
            return Context.Groups.Where(x => x.Status == true).
                                 Select(x => new SelectListItem()
                                 {
                                     Text = x.GroupName,
                                     Value = x.Id.ToString(),
                                     Selected = id == x.Id
                                 }).OrderBy(x => x.Text).ToList();
        }

        public List<USP_GetUsers_Model> GetUsers(UserModel model)
        {
            //GroupFilterModel groupFilterModel = new GroupFilterModel();
            return AutoMapper.Mapper.DynamicMap<List<USP_GetUsers_Model>>(Context.USP_GetUsers(model.UserName, model.FilterStatus, model.FromDate, model.ToDate).ToList()) ?? new List<USP_GetUsers_Model>();

            //return groupFilterModel;
        }

        public string ValidateUsers(UserModel model)
        {
            if (model != null)
            {
                if (Context.Users.Any(item => item.Email.ToLower() == model.Email.ToLower() && item.UserId != model.UserId && item.IsActive == false))
                    return model.FirstName + " " + model.LastName + " already exists, kindly restore";
                else if (Context.Users.Any(item => item.Email.ToLower() == model.Email.ToLower() && item.UserId != model.UserId))
                    return "Email Id already exists";

                else return string.Empty;

            }
            return string.Empty;
        }

        /// <summary>
        /// Method to Inactivate/Activate Status...
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Status"></param>
        /// <param name="loggedInUser"></param>
        public void SaveUser(UserModel userModel, int? loggedInUser)
        {
            if (userModel != null)
            {
                var isRegions = Context.UserRegions.Where(data => data.UserId == userModel.UserId).FirstOrDefault();
                if (isRegions != null)
                {
                    var userRegion = Context.UserRegions.Where(data => data.UserId == (int?)userModel.UserId).ToList();
                    foreach (var item in userRegion)
                    {
                        Context.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                        //Context.UserRegions.Attach(userRegion);
                        //  Context.UserRegions.Remove(userRegion);
                        Context.SaveChanges();
                    }
                }

                userModel.GroupId = (int?)Convert.ToUInt32(userModel.GroupNo);
                // userModel.UserRegions = null;
                var userData = Context.Users.Any(data => data.UserId == userModel.UserId);
                if (userData)
                {
                    User user = Context.Users.Find(userModel.UserId);
                    userModel.LastModifiedByUserId = loggedInUser;
                    userModel.LastModifedOnDate = DateTime.UtcNow;
                    userModel.CreatedOnDate = user.CreatedOnDate;
                    userModel.CreatedByUserId = user.CreatedByUserId;
                    Mapper.DynamicMap(userModel, user);
                }
                else
                {
                    userModel.CreatedByUserId = (int)loggedInUser;
                    userModel.LastName = userModel.LastName.RemoveExtraWhiteSpace();
                    userModel.CreatedOnDate = DateTime.UtcNow;
                    userModel.IsActive = true;
                    Context.Users.Add(Mapper.DynamicMap<User>(userModel));
                }

                Context.SaveChanges();

                var currentUser = Context.Users.Where(x => x.Email == userModel.Email).FirstOrDefault();
                if (currentUser != null)
                {
                    foreach (var item in userModel.UserRegionList)
                    {
                        var user = userModel.RegionList.Where(x => x.id == item.id).FirstOrDefault();
                        UserRegion uRegion = new UserRegion();
                        uRegion.UserId = currentUser.UserId;
                        uRegion.RegionId = item.id;
                        uRegion.RegionName = user.label;
                        Context.UserRegions.Add(Mapper.DynamicMap<UserRegion>(uRegion));

                    }
                    Context.SaveChanges();
                }
            }
        }



        /// <summary>
        /// Method to Inactivate/Activate Status...
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Status"></param>
        /// <param name="loggedInUser"></param>
        public void UpdateUserStatus(int ID, bool Status, int? loggedInUser)
        {
            if (Context.Users.Any(o => o.UserId == ID))
            {
                User UpdatedModel = Context.Users.Find(ID);
                UpdatedModel.LastModifiedByUserId = loggedInUser;
                UpdatedModel.LastModifedOnDate = DateTime.UtcNow;
                UpdatedModel.IsActive = Status;
                Context.SaveChanges();
            }


        }

        #endregion


        #region Forgot Password

        /// <summary>
        /// Method to validate user
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public UserModel ValidateEmail(UserModel userModel)
        {
            var userDetails = (from tableData in Context.Users
                               where tableData.Email == userModel.Email
                               select tableData).FirstOrDefault();
            return (Mapper.DynamicMap<User, UserModel>(userDetails));
        }


        /// <summary>
        /// Method to Reset Password
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public string ResetPassword(UserModel userModel)
        {
            var newPassword = Context.sp_GeneratePassword(6).ToList();
            User group = Context.Users.Find(userModel.UserId);
            group.Password = newPassword[0].ToString();
            Context.SaveChanges();
            return newPassword[0].ToString();
        }

        #endregion

        #region Layout
        public List<UserRights> GetUserRights(int userId)
        {
            var userRightsList = (from user in Context.Users
                                  join role in Context.Groups on user.GroupId equals role.Id
                                  join rr in Context.RightsToRoles on role.Id equals rr.RoleId
                                  join sm in Context.SubModules on rr.SubModuleId equals sm.id
                                  join module in Context.Modules on sm.ParentId equals module.id
                                  where user.UserId == userId
                                  select new UserRights
                                  {
                                      ModuleName = module.ModuleName,
                                      SubModuleName = sm.Name,
                                      Controller = sm.Controller,
                                      Action = sm.Action,
                                      Rights = rr.Rights,
                                      MinRight = sm.MinRight,
                                      ParentId=sm.ParentId,
                                      Sort=sm.Sort,
                                      IsMenu = sm.IsMenu
                                  }).OrderBy(y => y.ParentId).ThenBy(x => x.Sort).ToList();

            return userRightsList;

        }


        public List<UserRights> GetUserMenu(List<UserRights> rightList)
        {
            if (rightList != null)
                return rightList.Where(o => o.Rights > 0 && o.IsMenu == true).Select(item => new UserRights
                {
                    ModuleName = item.ModuleName,
                    SubModuleName = item.SubModuleName,
                    Controller = item.Controller,
                    Action = item.Action,
                    
                }).ToList();
            else
                return new List<UserRights>();
        }


        public List<UserRights> GetUserModules(List<UserRights> rightList)
        {
            if (rightList != null)
                return rightList.Distinct().Select(item => new UserRights
                {
                    ModuleName = item.ModuleName,
                }).ToList();
            else
                return new List<UserRights>();
        }
        #endregion




        #region Group
        public GroupModel GetGroup(int groupId)
        {
            var group = (from groups in Context.Groups
                         where groups.Id == groupId
                         select new GroupModel
                         {
                             Id = groups.Id,
                             GroupName = groups.GroupName
                         }).FirstOrDefault() ?? new GroupModel();


            group.RightsToRoleList = getRights(groupId);
            return group;

        }


        public List<USP_GetGroups_Model> GetGroups(GroupFilterModel model)
        {
            //GroupFilterModel groupFilterModel = new GroupFilterModel();
            return AutoMapper.Mapper.DynamicMap<List<USP_GetGroups_Model>>(Context.USP_GetGroups(model.GroupName, model.Status, model.FromDate, model.ToDate).ToList()) ?? new List<USP_GetGroups_Model>();

            //return groupFilterModel;
        }

        public string ValidateGroup(GroupModel model)
        {
            if (model != null)
            {
                if (Context.Groups.Any(item => item.GroupName.ToLower() == model.GroupName.ToLower() && item.Id != model.Id && item.Status == false))
                    return "Group Name already exists, kindly restore";
                else if (Context.Groups.Any(item => item.GroupName.ToLower() == model.GroupName.ToLower() && item.Id != model.Id))
                    return "Group Name already exists";

                else return string.Empty;

            }
            return string.Empty;
        }

        /// <summary>
        /// Method to Inactivate/Activate Status...
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Status"></param>
        /// <param name="loggedInUser"></param>
        public void SaveGroup(GroupModel groupModel, int? loggedInUser)
        {
            if (groupModel != null)
            {
                var groupData = Context.Groups.Any(data => data.Id == groupModel.Id);
                if (groupData)
                {
                    Group group = Context.Groups.Find(groupModel.Id);
                    group.LastModifiedByUserId = loggedInUser;
                    group.LastModifedOnDate = DateTime.UtcNow;
                    //Mapper.DynamicMap(groupModel, group);
                }
                else
                {
                    groupModel.GroupName = groupModel.GroupName.RemoveExtraWhiteSpace();
                    groupModel.CreatedByUserId = loggedInUser;
                    groupModel.CreatedOnDate = DateTime.UtcNow;
                    groupModel.Status = true;
                    Context.Groups.Add(Mapper.DynamicMap<Group>(groupModel));
                }

                Context.SaveChanges();
                var currentGroup = Context.Groups.Where(x => x.GroupName == groupModel.GroupName).FirstOrDefault();
                if (currentGroup != null)
                {
                    foreach (var item in groupModel.RightsToRoleList)
                    {
                        if (item.id > 0)
                        {
                            RightsToRole role = Context.RightsToRoles.Find(item.id);
                            role.RoleId = currentGroup.Id;
                            role.SubModuleId = item.SubModuleId;
                            role.Rights = item.Rights;
                            role.LastModifiedByUserId = loggedInUser;
                            role.LastModifedOnDate = DateTime.UtcNow;
                            role.Rights = item.Rights;
                            Context.SaveChanges();
                        }
                        else
                        {
                            RightsToRole right = new RightsToRole();
                            right.RoleId = currentGroup.Id;
                            right.SubModuleId = item.SubModuleId;
                            right.Rights = item.Rights;
                            right.CreatedByUserId = (int)loggedInUser;
                            right.CreatedOnDate = DateTime.UtcNow;
                            Context.RightsToRoles.Add(Mapper.DynamicMap<RightsToRole>(right));
                        }
                    }
                }


            }
        }

        public List<RightsToRoleModel> getRights(int roleId)
        {
            var rightList = (from submodule in Context.SubModules
                             join right in Context.RightsToRoles.Where(x => x.RoleId == roleId) on submodule.id equals right.SubModuleId into temp
                             from right in temp.DefaultIfEmpty()
                             where submodule.IsModule == true
                             select new RightsToRoleModel

               {
                   id = (int?)right.id ?? 0,
                   SubModuleId = submodule.id,
                   RoleId = (int?)roleId ?? 0,
                   ModuleName = submodule.Name,
                   Rights = (Int16?)right.Rights ?? 0,
                   ParentId = submodule.ParentId,

               }).ToList().OrderBy(x => x.ParentId);
            if (roleId > 0)
                return rightList.Where(o => o.RoleId == roleId).ToList();
            else
                return rightList.ToList();
        }

        /// <summary>
        /// Method to Inactivate/Activate Status...
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Status"></param>
        /// <param name="loggedInUser"></param>
        public void UpdateGroupStatus(int ID, bool Status, int? loggedInUser)
        {
            if (Context.Groups.Any(o => o.Id == ID))
            {
                Group UpdatedModel = Context.Groups.Find(ID);
                UpdatedModel.LastModifiedByUserId = loggedInUser;
                UpdatedModel.LastModifedOnDate = DateTime.UtcNow;
                UpdatedModel.Status = Status;
                Context.SaveChanges();
            }
        }

        #endregion

    }
}
