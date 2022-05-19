using Newtonsoft.Json;
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
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        public RoleType Type => (RoleType)Id;

        public override string ToString()
        {
            switch (Type)
            {
                case RoleType.admin:
                    return "Админ";
                case RoleType.financier:
                    return "Кассир";
                case RoleType.team_lead_media:
                    return "Медиа";
                case RoleType.team_lead_link:
                    return "Связка";
                case RoleType.team_lead_farm:
                    return "Фарм";
                case RoleType.team_lead_comment:
                    return "Комментарии";
                case RoleType.buyer_media:
                    return "Медиа";
                case RoleType.buyer_link:
                    return "Связка";
                case RoleType.buyer_farm:
                    return "Фарм";
                case RoleType.buyer_comment:
                    return "Комментарии";
                default:
                    return "?";                    
            }
        }
    }

}
