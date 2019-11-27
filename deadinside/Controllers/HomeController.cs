using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using deadinside.Models;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace deadinside.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        [Authorize]
        public ActionResult Index()
        {
            List<playeritem> playerItems = new List<playeritem>();
            List<item> items = new List<item>();
            List<player> players= new List<player>();

            //string conString = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;


            //using (MySqlConnection con = new MySqlConnection(conString))
            //{
            //    //todo distinct with joins?_?
            //    string query = "select * from items i " +
            //        "           inner join playeritems pi on pi.itemid = i.id " +
            //        "           inner join players p on p.id = pi.playerid" +
            //        "           order by p.id asc";

            //    using (MySqlCommand cmd = new MySqlCommand(query))
            //    {
            //        cmd.Connection = con;
            //        con.Open();
            //        using (MySqlDataReader sdr = cmd.ExecuteReader())
            //        {
            //            while (sdr.Read())
            //            {

            //                playerItems.Add(new playeritem
            //                {
            //                    id = Convert.ToInt32(sdr["id"]),
            //                    playerid = Convert.ToInt32(sdr["playerid"]),
            //                    itemid = Convert.ToInt32(sdr["itemid"]),
            //                    count = Convert.ToInt32(sdr["count"])
            //                });
            //                items.Add(new item
            //                {
            //                    id = Convert.ToInt32(sdr["id"]),
            //                    itemname = sdr["itemname"].ToString(),
            //                    itemdesc = sdr["itemdesc"].ToString()
            //                });
            //                players.Add(new player
            //                {
            //                    id = Convert.ToInt32(sdr["id"]),
            //                    playername = sdr["playername"].ToString()
            //                });
            //            }
            //        }
            //        con.Close();
            //    }
            //}
            using (runescapedatabaseEntities entity = new runescapedatabaseEntities())
            {
                players = entity.players.ToList<player>();
                playerItems = entity.playeritems.ToList<playeritem>();
                items = entity.items.ToList<item>();
            }
                //add playeritems to correct players playeritems list
            List<playeritem> distinctPlayerItems = playerItems.Distinct().ToList();
            List<player> distinctPlayers = players.GroupBy(x => x.playername).Select(grp => grp.First()).ToList();
            List<item> distinctItems = items.GroupBy(x => x.itemname).Select(grp => grp.First()).ToList();//items.Distinct().ToList();
            // ^this isnt the best but until i figure out a solution for the query this will do
            var viewModel = new playeritem();
            viewModel.playeritems = distinctPlayerItems;
            viewModel.items = distinctItems;
            viewModel.playerList = distinctPlayers; 
            return View(viewModel);
        }
    }
}