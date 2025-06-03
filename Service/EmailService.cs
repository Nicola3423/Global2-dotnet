// Services/EmailService.cs
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Sessions_app.Models;
using Sessions_app.Services;

namespace Sessions_app.Services
{
    public class EmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly bool _enableSsl;

        public EmailService(IConfiguration configuration)
        {
            var emailConfig = configuration.GetSection("EmailSettings");
            _smtpServer = emailConfig["SmtpServer"];
            _smtpPort = int.Parse(emailConfig["SmtpPort"]);
            _smtpUsername = emailConfig["SmtpUsername"];
            _smtpPassword = emailConfig["SmtpPassword"];
            _enableSsl = bool.Parse(emailConfig["EnableSsl"]);
        }

        public async Task EnviarEmailNotificacaoRisco(RiscoEvent riscoEvent)
        {
            // Aqui você pode buscar os destinatários do banco de dados
            // ou de configuração. Vou usar um email fixo como exemplo
            var destinatarios = new[] { "admin@empresa.com", "seguranca@empresa.com" };

            foreach (var email in destinatarios)
            {
                await EnviarEmail(
                    destinatario: email,
                    assunto: "Novo Risco Cadastrado",
                    corpo: $@"
                        <h1>Novo Risco Cadastrado no Sistema</h1>
                        <p><strong>ID:</strong> {riscoEvent.Id}</p>
                        <p><strong>Descrição:</strong> {riscoEvent.Descricao}</p>
                        <p><strong>Nível de Risco:</strong> {riscoEvent.Nivel}</p>
                        <p><strong>Data do Cadastro:</strong> {DateTime.Now:dd/MM/yyyy HH:mm}</p>
                        <br>
                        <p>Atenciosamente,<br>Sistema de Gestão de Riscos</p>
                    "
                );
            }
        }

        private async Task EnviarEmail(string destinatario, string assunto, string corpo)
        {
            using (var client = new SmtpClient(_smtpServer, _smtpPort))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
                client.EnableSsl = _enableSsl;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_smtpUsername),
                    Subject = assunto,
                    Body = corpo,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(destinatario);

                await client.SendMailAsync(mailMessage);
            }
        }
    }
}