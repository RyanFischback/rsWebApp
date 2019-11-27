using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using deadinside.Models;
using System.Data;

namespace deadinside.Controllers
{
    public class ItemController : Controller
    {
        // GET: Item
        public ActionResult Index()
        {
            List<item> itemsList = new List<item>();
            using(runescapedatabaseEntities entity = new runescapedatabaseEntities())
            {
                itemsList = entity.items.ToList<item>();
            }
            return View(itemsList);
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


        public ActionResult Added()
        {
            return View();
        }

        // POST: Item/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Added(int id, string playerName, FormCollection collection)
        {
            List<playeritem> playerItems = new List<playeritem>();
            //List<player> players = new List<player>();

            using (runescapedatabaseEntities entity = new runescapedatabaseEntities())
            {
                item itemModel = entity.items.Where(x => x.id == id).FirstOrDefault();
                var player = entity.players.Where(x => x.playername == playerName).FirstOrDefault();
                var itemCount = entity.playeritems.Where(x => x.itemid == itemModel.id).FirstOrDefault();
                playerItems = entity.playeritems.ToList<playeritem>();
                var exists = playerItems.Where(x => x.itemid == itemModel.id && x.playerid == player.id).FirstOrDefault(); // give me 

                if (exists != null)//if item id matches for a player..increment otherwise just add
                {
                    exists.count += 1;
                }
                else
                {
                    entity.playeritems.Add(new playeritem
                    {
                        playerid = player.id,
                        itemid = itemModel.id,
                        count = 1
                    });
                }
                //else // inital addition...i know they do the same thing. but if a player doesnt have any items they need to add them here to start off
                //{
                //    entity.playeritems.Add(new playeritem
                //    {
                //        playerid = player.id,
                //        itemid = itemModel.id,
                //        count = 1
                //    });

                //    playerItems.Add(new playeritem
                //    {
                //        playerid = player.id,
                //        itemid = itemModel.id,
                //        count = 1
                //    });
                //}


                entity.SaveChanges();
                //dont want to remove :)
            }
            return RedirectToAction("Added");

        }
    }
}
