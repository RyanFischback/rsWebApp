using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using deadinside.Models;
using System.Web.Security;
namespace deadinside.Controllers
  

//credit to https://www.youtube.com/watch?v=gSJFjuWFTdA tutorial
{
    public class PlayerController : Controller
    {
        //Registration Action
        [HttpGet]
        public ActionResult Registration()
        {
            return View(new player()); // makes it so player role can be default to normal...so that way not everyone can be an admin (different ways to do this but this works)
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration(player player)
        {
            bool Status = false;
            string message = "";

            if(ModelState.IsValid)
            {
                #region
                var isExist = IsPlayerExist(player.playername);
                if (isExist)
                {
                    ModelState.AddModelError("PlayerExist", "Player name is taken");
                    return View();
                }
                #endregion

                #region Password Hashing
                player.playerpassword = Crypto.Hash(player.playerpassword); // so password doesnt show up as the actual password in dbas
                //player.confirmPassword = Crypto.Hash(player.confirmPassword); // 
                #endregion

                #region Save to DB
                using (runescapedatabaseEntities entity = new runescapedatabaseEntities())
                {
                    entity.players.Add(new player
                    {
                        playername = player.playername,
                        playerpassword = player.playerpassword,
                        playerrole = "normal"
                    }); ;
                    try
                    {
                        entity.SaveChanges();
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                }
                #endregion

                message = "Registration Success";
                Status = true;

            }
            else
            {
                message = "Invalid Request";
            }


            ViewBag.Message = message;
            ViewBag.Status = Status;
            return View(player);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(PlayerLogin login, string ReturnUrl="")
        {
            string message = "";

            using (runescapedatabaseEntities entity = new runescapedatabaseEntities())
            {
                var v = entity.players.Where(x => x.playername == login.playername).FirstOrDefault();
                if(v != null)
                {
                    if(string.Compare(Crypto.Hash(login.Password), v.playerpassword) == 0)
                    {
                        int timeout = login.rememberMe ? 525600 : 60;
                        var ticket = new FormsAuthenticationTicket(login.playername, login.rememberMe, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;  // no js
                        Response.Cookies.Add(cookie);


                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        message = "Invalid Credentials";
                    }
                }
                else 
                {
                    message = "Invalid Credentials";
                }
            }

            ViewBag.Message = message;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Player");
        }

        [NonAction]
        public bool IsPlayerExist(string playerName)
        {
            using (runescapedatabaseEntities entity = new runescapedatabaseEntities())
            {
                var player = entity.players.Where(x => x.playername == playerName).FirstOrDefault();
                return player == null ? false : true;
            }
        }

        //[Authorize]
        //public ActionResult playerInventory(string playerName)
        //{
        //    using (runescapedatabaseEntities entity = new runescapedatabaseEntities())
        //    {
        //        // first develop the query
        //        // select * from playeritems pi
        //        //INNER JOIN players p on p.id = pi.playerid
        //        //INNER JOIN items i on i.id = pi.itemid
        //        //todo: conver to cs
        //    }
        //}
    }

}