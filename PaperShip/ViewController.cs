using System;
using System.Threading.Tasks;
using UIKit;

namespace PaperShip
{
    public partial class ViewController : UIViewController
    {
        Server server;
        MessageExchange message;

        protected ViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

		partial void UIButton402_TouchUpInside(UIButton sender)
		{
            server.Disconnected();
		}
        
		partial void UIButton199_TouchUpInside(UIButton sender)
		{
            server = Server.GetServer();
            message = new MessageExchange();
            message.SendRegistration(PhoneNumber.Text, UIDevice.CurrentDevice.Model);
            Task task = new Task(message.RecievedMessage);
            task.Start();
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}
