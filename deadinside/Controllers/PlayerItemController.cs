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
    public class PlayerItemController : Controller
    {
        public ActionResult Index()
        {
            //List<item> itemsList = new List<item>();
            //using (runescapedatabaseEntities entity = new runescapedatabaseEntities())
            //{
            //    itemsList = entity.items.ToList<item>();
            //}
            //return View(itemsList);// Specify the provider name, server and database.

            List<playeritem> playerItems = new List<playeritem>();
            List<item> items = new List<item>();
            List<player> players = new List<player>();
            var currentUserName = User.Identity.Name;
            string conString = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(conString))
            {
                string query = "select * from items i " +
                    "           inner join playeritems pi on pi.itemid = i.id " +
                    "           inner join players p on p.id = pi.playerid" +
                    "           where p.playername = @currentUserName" +
                    "           order by i.id asc";

                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@currentUserName", currentUserName); // where clause champ
                    cmd.Connection = con;
                    con.Open();
                    using (MySqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            playerItems.Add(new playeritem
                            {
                                id = Convert.ToInt32(sdr["id"]),
                                playerid = Convert.ToInt32(sdr["playerid"]),
                                itemid = Convert.ToInt32(sdr["itemid"]),
                                count = Convert.ToInt32(sdr["count"])
                            });
                            items.Add(new item
                            {
                                id = Convert.ToInt32(sdr["id"]),
                                itemname = sdr["itemname"].ToString(),
                                itemdesc = sdr["itemdesc"].ToString(),
                                itemURL = sdr["itemURL"].ToString(),
                                itemValue = Convert.ToInt32(sdr["itemValue"])
                            });
                            players.Add(new player
                            {
                                id = Convert.ToInt32(sdr["id"]),
                                playername = sdr["playername"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            var viewModel = new playeritem();
            viewModel.playeritems = playerItems;
            viewModel.items = items;
            viewModel.playerList = players;
            //inside of view is where i do queries to display everyones information one at a time. But i needed 3 views and this is how i saw to do it. Works for now (TODO: Look over & Change)
            return View(viewModel);
        }

        public ActionResult AdminIndex()
        {
            List<item> itemsList = new List<item>();
            using (runescapedatabaseEntities entity = new runescapedatabaseEntities())
            {
                itemsList = entity.items.ToList<item>();
            }
            return View(itemsList);
        }

        // GET: Item/Details/5
        public ActionResult Details(int id)
        {
            item itemModel = new item();
            using (runescapedatabaseEntities entity = new runescapedatabaseEntities())
            {
                itemModel = entity.items.Where(x => x.id == id).FirstOrDefault();
            }
            return View(itemModel);
        }

        // GET: Item/Create
        public ActionResult Create()
        {

            return View(new item());
        }

        // POST: Item/Create
        [HttpPost]
        public ActionResult Create(item itemModel)
        {
            using (runescapedatabaseEntities entity = new runescapedatabaseEntities())
            {
                entity.items.Add(itemModel);
                entity.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // GET: Item/Edit/5
        public ActionResult Edit(int id)
        {
            item itemModel = new item();
            using (runescapedatabaseEntities entity = new runescapedatabaseEntities())
            {
                itemModel = entity.items.Where(x => x.id == id).FirstOrDefault();
            }
            return View(itemModel);
        }

        // POST: Item/Edit/5
        [HttpPost]
        public ActionResult Edit(item itemModel)
        {
            using (runescapedatabaseEntities entity = new runescapedatabaseEntities())
            {
                entity.Entry(itemModel).State = System.Data.Entity.EntityState.Modified;
                entity.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // GET: Item/Delete/5
        public ActionResult Delete(int id)
        {
            item itemModel = new item();
            using (runescapedatabaseEntities entity = new runescapedatabaseEntities())
            {
                itemModel = entity.items.Where(x => x.id == id).FirstOrDefault();
            }
            return View(itemModel);
        }

        // POST: Item/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            using (runescapedatabaseEntities entity = new runescapedatabaseEntities())
            {
                item itemModel = entity.items.Where(x => x.id == id).FirstOrDefault();
                entity.items.Remove(itemModel);
                entity.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Sold()//item soldItem)
        {
            //var viewModel = new playeritem();
            //viewModel.items.Add(soldItem);
            return View();//viewModel);
        }

        // POST: Item/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sold(int id, string playerName, FormCollection collection)
        {
            List<playeritem> playerItems = new List<playeritem>();
            List<item> items = new List<item>();
            //List<player> players = new List<player>();

            using (runescapedatabaseEntities entity = new runescapedatabaseEntities())
            {
                item itemToRemove = entity.items.Where(x => x.id == id).FirstOrDefault();
                //items.Add(itemToRemove); // passing this to html
                item itemModel = entity.items.Where(x => x.itemname == "Coins").FirstOrDefault();
                var player = entity.players.Where(x => x.playername == playerName).FirstOrDefault();
                var itemCount = entity.playeritems.Where(x => x.itemid == itemModel.id).FirstOrDefault();
                playerItems = entity.playeritems.ToList<playeritem>();
                var finalRemove = playerItems.Find(x => x.itemid == itemToRemove.id && x.playerid == player.id); // remove item
                var exists = playerItems.Where(x => x.itemid == itemModel.id && x.playerid == player.id).FirstOrDefault(); // if i already have money..ad more to it
                //entity.playeritems.Remove(finalRemove);
                //increase count based on item value
                if (exists != null)//if item id matches for a player..increment otherwise just add
                {
                    if (finalRemove.count >= 2) // if they have more than 1 dont remove the item
                    {
                        //exists.playeritems.Remove(finalRemove); 
                        finalRemove.count -= 1;
                    }
                    else
                    {
                        entity.playeritems.Remove(finalRemove); // if there removing with 1 of that item...delete it
                    }
                    exists.count += itemToRemove.itemValue; // count may be kept alone, used as count (not to count money) and may have another column for player money
                }
                else
                {
                    entity.playeritems.Add(new playeritem
                    {
                        playerid = player.id,
                        itemid = itemModel.id,
                        count = itemToRemove.itemValue
                    });

                }
                entity.SaveChanges();
            }
            return RedirectToAction("Sold");//, "PlayerItem", new {item = items[0]});

        }
    }
}
