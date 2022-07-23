using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarderieManagementClean.Application.Models
{
    public class Result<T> where T : class
    {
        public bool Success { get; set; } = false;
        public object Data { get; set; } = null;
        public IEnumerable<string> Errors { get; set; } = null;
    }
}
