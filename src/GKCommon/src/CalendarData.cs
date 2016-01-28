﻿using System;
using System.Globalization;
using System.Threading;

namespace GKCommon
{
    public static class CalendarData
    {

        public static string date_to_str(int year, int month, int day, CalendarConverter.DateEra era)
		{
			DateTimeFormatInfo dtInfo = Thread.CurrentThread.CurrentCulture.DateTimeFormat;

			string result = string.Concat(new string[] {
				day.ToString(),  " ",  dtInfo.AbbreviatedMonthNames[month - 1],  " ",  year.ToString()
			});

			if (era != CalendarConverter.DateEra.AD) {
				result += " до н.э.";
			}

			return result;
		}

		public static readonly string[] HebrewMonths;
		public static readonly string[] HebrewWeekdays;
		public static readonly string[] IslamicMonths;
		public static readonly string[] IslamicWeekdays;
		public static readonly string[] PersianMonths;
		public static readonly string[] PersianWeekdays;
		public static readonly string[] IndianCivilMonths;
		public static readonly string[] IndianCivilWeekdays;
		public static readonly string[] BahaiMonths;
		public static readonly string[] BahaiWeekdays;

		static CalendarData()
		{
			CalendarData.BahaiWeekdays = new string[]
			{
				"Джамаль", 
				"Камаль", 
				"Фидаль", 
				"Идаль", 
				"Истиджлаль", 
				"Истиклаль", 
				"Джалаль"
			};

			CalendarData.BahaiMonths = new string[]
			{
				"Бахa", 
				"Джалaл", 
				"Джамaл", 
				"Азамат", 
				"Нур", 
				"Рахмат", 
				"Калимaт", 
				"Камaл", 
				"Асмa", 
				"Иззат", 
				"Машиййат", 
				"Ильм", 
				"Кудрат", 
				"Каул", 
				"Масa’иль", 
				"Шараф", 
				"Султан", 
				"Мульк", 
				"Аййaм-и Хa", 
				"Алa"
			};

			CalendarData.IndianCivilWeekdays = new string[]
			{
				"равивар", 
				"сомвар", 
				"мангалвар", 
				"будхвар", 
				"брихаспативар", 
				"шукрвар", 
				"шанивар"
			};

			CalendarData.IndianCivilMonths = new string[]
			{
				"Чайтра", 
				"Ваисакха", 
				"Джанштха", 
				"Асадха", 
				"Сравана", 
				"Бхадра", 
				"Азвина", 
				"Картика", 
				"Аграхайана", 
				"Пауза", 
				"Магха", 
				"Пхалгуна"
			};

			CalendarData.PersianWeekdays = new string[]
			{
				"йекшанбе", 
				"душанбе", 
				"сешанбе", 
				"чахаршанбе", 
				"панджшанбе", 
				"джоме", 
				"шанбе"
			};

			CalendarData.PersianMonths = new string[]
			{
				"Фарвардин", 
				"Ордибехешт", 
				"Хордад", 
				"Тир", 
				"Мордад", 
				"Шахривар", 
				"Мехр", 
				"Абан", 
				"Азар", 
				"Дей", 
				"Бахман", 
				"Эсфанд"
			};

			CalendarData.IslamicWeekdays = new string[]
			{
				"аль-ахад", 
				"аль-иснайн", 
				"ас-саласа'", 
				"аль-арба'а", 
				"аль-хамис", 
				"аль-джум'а", 
				"ас-сабт"
			};

			CalendarData.IslamicMonths = new string[]
			{
				"мухаррам", 
				"сафар", 
				"рабии`у ль-авваль", 
				"рабии`у с-саании", 
				"джумаада ль-ууля", 
				"джумаада ль-аахыр", 
				"раджаб", 
				"шаабан", 
				"рамадан", 
				"шавваль", 
				"зуль-ка`да", 
				"зульхиджа"
			};

			CalendarData.HebrewWeekdays = new string[]
			{
				"алеф", 
				"бейт", 
				"гимел", 
				"далет", 
				"хей", 
				"вав", 
				"зайин"
			};

			CalendarData.HebrewMonths = new string[]
			{
				"Нисан", 
				"Ияр", 
				"Сиван", 
				"Тамуз", 
				"Ав", 
				"Элул", 
				"Тишрей", 
				"Хешван", 
				"Кислев", 
				"Тевет", 
				"Шват", 
				"Адар", 
				"Адар бет"
			};
		}

    }
}
