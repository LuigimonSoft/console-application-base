using System;
using ConsoleBase.Common.Attributes;

namespace ConsoleBase.Core.Models
{
    public class BaseModel
    {
      [Column(0)]
      public string Name { get; set; } 
      [Column(1)]
      public int Age { get; set; }
      [Column(2)]
      public decimal Salary { get; set; } 
      [Column(3)]
      public string FilePath { get; set; }
    }
}