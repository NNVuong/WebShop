using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace WebShop.Models
{
    public static class Extensions
    {
        static Random rnd = new Random();

        public static IEnumerable<T> PickRandom<T>(this IList<T> source, int count)
        {
            return source.OrderBy(x => rnd.Next()).Take(count);
        }
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) :
                JsonConvert.DeserializeObject<T>(value);
        }
    }
}
