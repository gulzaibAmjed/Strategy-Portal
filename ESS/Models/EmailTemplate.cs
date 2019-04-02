using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;


namespace ESS.Models
{
    public class EmailTemplate
    {
        public int TEMPLATE_ID { get; set; }
        public int TEMPLATE_CODE { get; set; }
        public int LANGUAGE_ID { get; set; }
        public string MESSAGE_TITLE { get; set; }
        [Required]
        [AllowHtml]
        [UIHint("tinymce_full_compressed")]
        [Display(Name = "Page Content")]
        public string PageContent { get; set; }
        public string MESSAGE_SIGNATURE { get; set; }
        public string MESSAGE_SENDER { get; set; }
        public List<string> LanguageList { get; set; }

    }
}