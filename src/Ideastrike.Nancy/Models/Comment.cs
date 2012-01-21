using System;
using System.ComponentModel.DataAnnotations;
namespace Ideastrike.Nancy.Models
{
    public class Comment : Activity
    {
        public string Text { get; set; }
    }
}