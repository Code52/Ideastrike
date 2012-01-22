namespace Ideastrike.Nancy.Helpers
{
    /// <summary>
    /// This class is used to return the proper JSON needed by the jQuery Image uploader
    /// </summary>
    internal class ImageFileStatus
    {
        public string group { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public int size { get; set; }
        public string progress { get; set; }
        public string url { get; set; }
        public string thumbnail_url { get; set; }
        public string delete_url { get; set; }
        public string delete_type { get; set; }
        public string error { get; set; }
        public string imageId { get; set; }

        public ImageFileStatus(int imageId, int fileLength, string name)
        {
            this.SetValues(imageId, fileLength, name);
        }

        private void SetValues(int imageId, int fileLength, string name)
        {
            this.name = name;
            type = "image/png";
            size = fileLength;
            progress = "1.0";
            url = "/idea/image/" + imageId;
            thumbnail_url = "/idea/imagethumb/" + imageId + "/100";
            delete_url = "/idea/deleteimage/" + imageId;
            delete_type = "DELETE";
            this.imageId = imageId.ToString();
        }
    }
}