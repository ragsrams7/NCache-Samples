﻿// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

using System;
using System.Collections;
using Alachisoft.NCache.Web.Caching;
using Alachisoft.NCache.Sample.Data;
using System.Configuration;

namespace Alachisoft.NCache.Samples
{
    /// <summary>
    /// Class that demonstrates the usage of groups in NCache
    /// </summary>
    public class Groups
    {
        private static Cache _cache;

        public static void Run()
        {
            // Initialize the cache
            InitializeCache();

            //Adding item in same group
            //Group can be done at two levels 
            //Groups and Subgroups.          
            AddItems();

            // Getting group data
            // Will return nine items since no subgroup is defined;
            GetItemsByGroup();

            // Getting sub group data
            // Will return three items under the subgroup Mobiles
            GetItemsBySubGroup();

            // getGroupKeys is yet another function to retrive group data.
            // It however requires multiple iterations to retrive actual data
            // 1) To get List of Keys and 2) TO get items for the return List of Keys

            // Updating items in groups
            // Item is updated at the specified group
            UpdateItem();

            // Removing sub group data
            RemoveSubGroupData();

            // Remove group data
            RemoveGroupData();

            // Dispose cache once done
            _cache.Dispose();
        }

        /// <summary>
        /// This method initializes the cache.
        /// </summary>
        private static void InitializeCache()
        {
            string cache = ConfigurationManager.AppSettings["CacheId"];

            if (String.IsNullOrEmpty(cache))
            {
                Console.WriteLine("The Cache Name cannot be null or empty.");
                return;
            }

            // Initialize an instance of the cache to begin performing operations:
            _cache = NCache.Web.Caching.NCache.InitializeCache(cache);

            Console.WriteLine("Cache initialized successfully");
        }

        /// <summary>
        /// This method adds items in the cache.
        /// </summary>
        private static void AddItems()
        {
            AddGroupedDatatoCache(1, "HTCPhone", "Electronics","Mobiles");
            AddGroupedDatatoCache(2, "SamsungPhone", "Electronics", "Mobiles");
            AddGroupedDatatoCache(3, "NokiaPhone", "Electronics", "Mobiles");

            AddGroupedDatatoCache(4, "AcerLaptop", "Electronics", "Laptops");
            AddGroupedDatatoCache(5, "HPLaptop", "Electronics", "Laptops");
            AddGroupedDatatoCache(6, "DellLaptop", "Electronics", "Laptops");


            AddGroupedDatatoCache(7, "HairDryer", "Electronics", "SmallElectronics");
            AddGroupedDatatoCache(8, "VaccumCleaner", "Electronics", "SmallElectronics");
            AddGroupedDatatoCache(9, "Iron", "Electronics", "SmallElectronics");

       
            Console.WriteLine("Items added in cache.");
        }





        private static void AddGroupedDatatoCache(int productId, string productName, string group,string subGroup, string className = "Elec", string category = "Elec")
        {
            Product product = new Product() { Id = productId, Name = productName, ClassName = className, Category = category };

            // create object key
            string key = "product:" + product.Id.ToString();

            // add data in cache 
            _cache.Add(key, product,group, subGroup);
        }




        /// <summary>
        /// This method fetches items from the cache using groups
        /// </summary>
        private static void GetItemsByGroup()
        {
            IDictionary items = _cache.GetGroupData("Electronics", null);
            if (items.Count > 0)
            {
                Console.WriteLine("Item count: " + items.Count);
                Console.WriteLine("Following Products are found in group 'Electronics'");
                IEnumerator itor = items.Values.GetEnumerator();
                while (itor.MoveNext())
                {
                    Console.WriteLine(((Product)itor.Current).Name);
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// This method fetches items from the cache using sub groups
        /// </summary>
        private static void GetItemsBySubGroup()
        {
            IDictionary items = _cache.GetGroupData("Electronics", "Mobiles");
            if (items.Count > 0)
            {
                Console.WriteLine("Item count: " + items.Count);
                Console.WriteLine("Following Products are found in group 'Electronics' and Subgroup 'Mobiles'");
                IEnumerator itor = items.Values.GetEnumerator();
                while (itor.MoveNext())
                {
                    Console.WriteLine(((Product)itor.Current).Name);
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// This mehtod updates items in the cache.
        /// </summary>
        private static void UpdateItem()
        {
            Product product =  new Product() { Id = 9, Name = "PanaSonicIron" };
            string key = "product:" + product.Id.ToString();
            _cache.Insert(key, product, "Electronics", "SmallElectronics");
        }

        /// <summary>
        /// This method removes items from the cache that belong to a specific sub group
        /// </summary>
        private static void RemoveSubGroupData()
        {
            Console.WriteLine("Item count: " + _cache.Count); // Itemcount = 9
            _cache.RemoveGroupData("Electronics", "Mobiles"); // Will remove 3 items from cache based on subgroup Mobiles
        }

        /// <summary>
        /// This method removes items from the cache that belong to a specific group
        /// </summary>
        private static void RemoveGroupData()
        {
            Console.WriteLine("Item count: " + _cache.Count); // Itemcount = 6
            _cache.RemoveGroupData("Electronics", null); // Will remove all items from cache based on group Electronics

            Console.WriteLine("Item count: " + _cache.Count); // Itemcount = 0
        }
    }
}