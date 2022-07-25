using GarderieManagementClean.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Group = GarderieManagementClean.Domain.Entities.Group;

namespace Contracts.Dtos.Response
{
    public class EnfantResponse
    {

        public int Id { get; set; }

        public string Nom { get; set; }

        public DateTime DateNaissance { get; set; }

        // public string Photo { get; set; } = "https://cdn.vox-cdn.com/thumbor/23dWY86RxkdF7ZegvfnY8gFjR7s=/1400x1400/filters:format(jpeg)/cdn.vox-cdn.com/uploads/chorus_asset/file/19157811/ply0947_fall_reviews_2019_tv_anime.jpg";

        public string Image { get; set; } = "https://cdn.vox-cdn.com/thumbor/23dWY86RxkdF7ZegvfnY8gFjR7s=/1400x1400/filters:format(jpeg)/cdn.vox-cdn.com/uploads/chorus_asset/file/19157811/ply0947_fall_reviews_2019_tv_anime.jpg";


        public GroupDTO Group { get; set; }




    }
}
