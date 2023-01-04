namespace BusinessObjects.WebPushManager
{
    public class WebPushClientModel
    {
        public WebPushClientModel()
        {
        }

        public WebPushClientModel(int id, string userName, string endpoint, string p256DH, string auth)
        {
            PushEndpoint = endpoint;
            P256DH = p256DH;
            Auth = auth;
            UserName = userName;
            Id = id;
        }
        public int Id { get; set; }
        public string PushEndpoint { get; set; }
        public string P256DH { get; set; }
        public string Auth { get; set; }
        public string UserName { get; set; }
    }
}