﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.Models.user
{
    public enum RoleType : int
    {
        admin = 1,
        user,
        financier,
        team_lead_media,
        team_lead_link,
        team_lead_farm,
        team_lead_comment,
        buyer_media,
        buyer_link,
        buyer_farm,
        buyer_comment,
        creative
    }

    public class Role
    {
        public const string admin = "Админ";
        public const string financier = "Кассир";
        public const string comment = "Комментарии";
        public const string creative = "Креативщик";
        public const string media = "Медиа";
        public const string teamlead = "Тим-лид";
        public const string link = "Связка";
        public const string farm = "Фарм";

        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }        
        public RoleType Type { get; }

        public Role(RoleType type)
        {
            Type = type;
            Id = (int)type;
            Name = type.ToString();
        }

        public Role(int id)
        {
            Type = (RoleType)id;
            Id = id;
            Name = Type.ToString();
        }

        public Role()
        {
        }    
       
    }

}
