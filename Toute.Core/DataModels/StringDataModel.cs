using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Toute.Core
{
    /// <summary>
    /// string data model, that help binds
    /// strings values to DB
    /// </summary>
    public class StringDataModel
    {
        /// <summary>
        /// Id of the item
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        /// <summary>
        /// Value of the item
        /// </summary>
        public string Value { get; set; }
    }
}
