using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Models
{
    internal class About
    {
        public string Title => AppInfo.Name;

        public string Nombre => "Mateo Pillajo Guijarro";
        public string Version => AppInfo.VersionString;
        public string MoreInfoUrl => "https://github.com/mateop11";
        public string Message => "Mi nombre es Mateo, tengo 22 años y estudio Ingenieria de Software, practco box y me apasiona mucho el fútbol.";
    }
}
