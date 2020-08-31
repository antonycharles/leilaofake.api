using LeilaoFake.Me.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace LeilaoFake.Me.Test.Models
{
    public class UsuarioConstrutorTest
    {
        [Fact]
        public void LancaArgumentNullExceptionUsuarioIdNull()
        {
            Assert.Throws<ArgumentNullException>(
                //Act
                () => new Usuario(null, "Camila Silva")
            );
        }
    }
}
