using RadioApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RadioApp.Converter
{
    public static class Mapper
    {
        public static Favorite Map(RadioStation rs)
        {
            Favorite favorite = new Favorite()
            {
                Slug = rs.Slug,
                Title = rs.Title,
                Bitrate = rs.Bitrate,
                Country = rs.Country,
                StreamUrl = rs.StreamUrl,
                Subtext = rs.Subtext,
                Image = rs.Image,
                Lang = rs.Lang
            };
            return favorite;
        }

        public static RadioStation Map(Favorite rs)
        {
            RadioStation radioStation = new RadioStation()
            {
                Slug = rs.Slug,
                Title = rs.Title,
                Bitrate = rs.Bitrate,
                Country = rs.Country,
                StreamUrl = rs.StreamUrl,
                Subtext = rs.Subtext,
                Image = rs.Image,
                Lang = rs.Lang,
                Favorite = true
            };
            return radioStation;
        }
    }
}
