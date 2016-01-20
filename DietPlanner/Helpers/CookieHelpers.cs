using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DietPlanner.Helpers
{
    public class NullCookieException : Exception
    {
        public NullCookieException() : base("Próba dostępu do nieistniejącego ciasteczka!") { }
        public NullCookieException(string cookieName) : base("Ciasteczko o nazwie " + cookieName + " nie istnieje!") { }
    }

    public static class CookieHelper
    {
        public static void CreateCookie(string name, string propertyName, string propertyValue, TimeSpan expirationTime)
        {
            HttpCookie cookie = new HttpCookie(name);
            cookie[propertyName] = propertyValue;
            cookie.Expires = DateTime.Now.Add(expirationTime);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        public static void CreateCookie(string name, IDictionary<string, string> properties, TimeSpan expirationTime)
        {
            HttpCookie cookie = new HttpCookie(name);
            foreach (var property in properties)
            {
                cookie[property.Key] = property.Value;
            }
            cookie.Expires = DateTime.Now.Add(expirationTime);
            HttpContext.Current.Response.AppendCookie(cookie);
        }
        
        public static void CreateCookie<T>(string name, string propertyName, T propertyValue, TimeSpan expirationTime)
        {
            HttpCookie cookie = new HttpCookie(name);
            cookie[propertyName] = JsonConvert.SerializeObject(propertyValue);
            cookie.Expires = DateTime.Now.Add(expirationTime);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        public static HttpCookie GetCookie(string name)
        {
            return HttpContext.Current.Request.Cookies[name];
        }

        public static HttpCookie GetOrCreateCookie(string name, TimeSpan expirationTime)
        {
            if (CookieExists(name)) 
            { 
                return GetCookie(name); 
            }
            else 
            {
                HttpCookie cookie = new HttpCookie(name);
                cookie.Expires = DateTime.Now.Add(expirationTime);
                HttpContext.Current.Response.AppendCookie(cookie);
                return cookie;
            }
        }
        
        public static bool CookieExists(string name)
        {
            return HttpContext.Current.Request.Cookies[name] != null;
        }

        public static bool CookieExists(string name, out HttpCookie cookie)
        {
            cookie = HttpContext.Current.Request.Cookies[name];
            return cookie != null;
        }

        public static void DeleteCookie(string name)
        {
            if (HttpContext.Current.Request.Cookies[name] != null)
            {
                HttpCookie cookie = new HttpCookie(name);
                cookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.AppendCookie(cookie);
            }
        }

        public static void UpdateCookie(string name, string property, string value)
        {
            UpdateCookie(GetCookie(name), property, value);
        }

        public static void UpdateCookie(HttpCookie cookie, string property, string value)
        {
            if (cookie == null) { throw new NullCookieException(); }
            cookie[property] = value;
            HttpContext.Current.Response.SetCookie(cookie);
        }

        public static void UpdateCookie<T>(string name, string property, T value)
        {
            UpdateCookie<T>(GetCookie(name), property, value);
        }
        
        public static void UpdateCookie<T>(HttpCookie cookie, string property, T value)
        {
            if (cookie == null) { throw new NullCookieException(); }
            cookie[property] = JsonConvert.SerializeObject(value);
            HttpContext.Current.Response.SetCookie(cookie);
        }

        public static string ReadCookie(string name, string property)
        {
            return ReadCookie(GetCookie(name), property);
        }

        public static string ReadCookie(HttpCookie cookie, string property)
        {
            if (cookie == null) { throw new NullCookieException(); }
            return cookie[property] ?? string.Empty;
        }

        public static T ReadCookie<T>(string name, string property)
        {
            return ReadCookie<T>(GetCookie(name), property);
        }

        public static T ReadCookie<T>(HttpCookie cookie, string property)
        {
            if (cookie == null) { throw new NullCookieException(); }
            return JsonConvert.DeserializeObject<T>(cookie[property] ?? string.Empty);
        }

        public static bool TryReadCookie<T>(string name, string property, out T result)
        {
            return TryReadCookie<T>(GetCookie(name), property, out result);
        }

        public static bool TryReadCookie<T>(HttpCookie cookie, string property, out T result)
        {
            result = default(T);
            if (cookie == null) { return false; }
            try 
            { 
                result = JsonConvert.DeserializeObject<T>(cookie[property]); return true;
            }
            catch (Exception) { return false; }
        }

        public static void SetValue(this HttpCookie cookie, string property, string value)
        {
            cookie[property] = value;
        }

        public static void SetValue<T>(this HttpCookie cookie, string property, T value)
        {
            cookie[property] = JsonConvert.SerializeObject(value);
        }

        public static void Save(this HttpCookie cookie)
        {
            HttpContext.Current.Response.SetCookie(cookie);
        }
    }
}