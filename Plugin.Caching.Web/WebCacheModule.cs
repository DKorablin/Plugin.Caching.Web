using System;
using System.Web;
using SAL.Interface.Caching;
using System.Collections.Generic;

namespace Plugin.Caching.Web
{
	public class WebCacheModule : ICacheModule
	{
		private static readonly Object CacheLock = new Object();

		private static WebCacheModule CacheInstance = null;

		/// <summary>Локер вызовов методов для получения данных из источника данных</summary>
		private readonly Dictionary<String, Object> _lockMethodCall = new Dictionary<String, Object>();

		internal static Boolean IsValid => HttpRuntime.Cache != null;

		internal static WebCacheModule Instance
		{
			get
			{
				if(CacheInstance == null)
					lock(CacheLock)
						CacheInstance = CacheInstance ?? (CacheInstance = new WebCacheModule());
				return CacheInstance;
			}
		}

		/// <summary>Cache module name</summary>
		public String Name => "HttpRuntime.Cache";

		internal System.Web.Caching.Cache Cache => HttpRuntime.Cache;

		private WebCacheModule() { }

		public T Get<T>(String key) where T : class
			=> (T)this.Cache[key];

		/// <summary>Получить объект из кеша</summary>
		/// <typeparam name="T">Тип объекта получаемого из кеша</typeparam>
		/// <param name="key">Ключ по которому получить объект</param>
		/// <param name="fallback">Функция обратного вызова получения данных, если данных нет в кеше</param>
		/// <param name="slidingExpiration">Плавающий временной интервал жизни объекта в кеше</param>
		/// <param name="absoluteExpiration">Фиксированная дата и время жизни объекта в кеше</param>
		/// <returns>Полученный объект из кеша</returns>
		public T Get<T>(String key, Func<T> fallback, TimeSpan? slidingExpiration, DateTimeOffset? absoluteExpiration) where T : class
		{
			T result = this.Get<T>(key);
			if(result == null && fallback != null)
			{
				if(!this._lockMethodCall.TryGetValue(key, out Object lockMethod))//Лок на метод получения данных
					lock(this._lockMethodCall.Keys)
						if(!this._lockMethodCall.TryGetValue(key, out lockMethod))
							this._lockMethodCall.Add(key, lockMethod = new Object());

				lock(lockMethod)
				{
					result = this.Get<T>(key);
					if(result == null)
					{
						result = fallback();
						this.Add(key, result, slidingExpiration, absoluteExpiration);
					}
				}
			}

			return result;
		}

		/// <summary>Положить объект в кеш</summary>
		/// <typeparam name="T">Тип объекта кидаемого в кеш</typeparam>
		/// <param name="key">Ключ, по которому записать объект в кеш</param>
		/// <param name="value">Данные для добавления в кеш</param>
		/// <param name="slidingExpiration">Плавающий временной интервал жизни объекта в кеше</param>
		/// <param name="absoluteExpiration">Фиксированная дата и время жизни объекта в кеше</param>
		public void Add<T>(String key, T value, TimeSpan? slidingExpiration, DateTimeOffset? absoluteExpiration)
		{
			this.Cache.Add(key,
				value,
				null,
				absoluteExpiration == null ? System.Web.Caching.Cache.NoAbsoluteExpiration : absoluteExpiration.Value.DateTime,
				slidingExpiration == null ? System.Web.Caching.Cache.NoSlidingExpiration : slidingExpiration.Value,
				System.Web.Caching.CacheItemPriority.Normal,
				null);
		}

		public void Remove(String key)
			=> this.Cache.Remove(key);
	}
}