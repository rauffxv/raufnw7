namespace raufnw7
{
    using System.ComponentModel.DataAnnotations;
    using System.Net;
    using System.Net.Mail;
    using System.Threading.Channels;

    internal class Program
    {
        static string fromMail = " ";
        static string appPassword = " ";

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("-----------Registration-----------");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Name: ");
            string name = Console.ReadLine();

            Console.Write("Surname: ");
            string surname = Console.ReadLine();

            Console.Write("Email: ");
            string toMail = Console.ReadLine();

            Console.Write("Password: ");
            string password = Console.ReadLine();
            Console.ResetColor();


            Random rnd = new Random();
            string generatedOtp = rnd.Next(100000, 999999).ToString();

            var mail = new MailMessage();
            mail.From = new MailAddress(fromMail);

            mail.To.Add(new MailAddress(toMail));
            mail.Subject = "OTP";

            mail.Body = $"OTP code: {generatedOtp}";
            var smtp = new SmtpClient("smtp.gmail.com");
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential(fromMail, appPassword);

            try
            {
                smtp.Send(mail);
                Console.WriteLine("OTP code sent successfully..");
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error: " + ex.Message);

            }

            DateTime expiryTime = DateTime.Now.AddMinutes(3);
            int maxAttempts = 3;
            bool isVerified = false;

            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                // Vaxt bitib-bitmədiyini yoxlayırıq
                if (DateTime.Now > expiryTime)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nTime expired! The 3-minute limit for OTP verification has passed");
                    Console.ResetColor();
                    break;
                }

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"\nOTP code (Attempt {attempt}/{maxAttempts}): ");
                string otp = Console.ReadLine();
                Console.ResetColor();

                if (otp == generatedOtp)
                {
                    isVerified = true;
                    break;
                }
                else
                {
                    int remainingAttempts = maxAttempts - attempt;
                    Console.ForegroundColor = ConsoleColor.Red;
                    if (remainingAttempts > 0)
                    {
                        Console.WriteLine($"Invalid code! Remaining attempts: {remainingAttempts}");
                    }
                    Console.ResetColor();
                }

                if (isVerified)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nRegistration completed successfully...");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nRegistration failed. Max attempts reached or time expired.");
                    Console.ResetColor();
                }
            }
        }
    }
}
