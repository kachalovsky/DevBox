//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DevBox.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Answers
    {
        public int Id { get; set; }
        public Nullable<int> Id_Question { get; set; }
        public string Answer { get; set; }
    
        public virtual Question Question { get; set; }
    }
}
