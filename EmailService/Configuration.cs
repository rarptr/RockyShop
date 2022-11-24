using System;
using System.Collections.Generic;
using System.Text;

namespace EmailService
{
    // Содержит свойства, необходимые для настройки отправки сообщений электронной почты
    // из приложения. Для заполнения свойств используется файл appsettings.json
    public class Configuration
    {
        public string From { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
