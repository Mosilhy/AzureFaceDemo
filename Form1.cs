
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;


namespace FaceRecognitionNetways
{
    public partial class Form1 : Form
    {
        private static readonly string personGroupId = Guid.NewGuid().ToString();

        // URL path for the images.
        private const string IMAGE_BASE_URL = "https://csdx.blob.core.windows.net/resources/Face/Images/";

        // From your Face subscription in the Azure portal, get your subscription key and endpoint.
        private const string SUBSCRIPTION_KEY = "PUT YOUR KEY HERE !!";
        private const string ENDPOINT = "Put the ENd point here !!! ";
        private string[] files;
        private int CurrentImage = 0;
        private readonly FaceClient? faceClient;

        public Form1()
        {
            InitializeComponent();

            faceClient = new(new ApiKeyServiceClientCredentials(SUBSCRIPTION_KEY))
            {
                Endpoint = ENDPOINT
            };
        }

        private async Task UpdateUIWithFaceFeatures(FaceClient faceClient, string detectedFace)
        {
            pictureBox1.Image = new Bitmap(detectedFace);
            Genderlbl.Text = "";

            AgeValue.Text = "";


            HappinessValue.Text = "";

            SadnessValue.Text = "";


            FaceAttributeType[]? requiredFaceAttributes = new FaceAttributeType[] {
            FaceAttributeType.Age,
            FaceAttributeType.Gender,
            FaceAttributeType.Smile,
            FaceAttributeType.Emotion,
            FaceAttributeType.Hair,
};
            IList<DetectedFace> DetectedFaces;
            using (Stream imageStream = File.OpenRead(detectedFace))
            {
                DetectedFaces = await faceClient.Face.DetectWithStreamAsync(imageStream, false, false, recognitionModel: "recognition_01", returnFaceAttributes: requiredFaceAttributes);

            }
            Genderlbl.Text = DetectedFaces[0].FaceAttributes.Gender.ToString();

            AgeValue.Text = DetectedFaces[0].FaceAttributes.Age.ToString();

            HappinessValue.Text = DetectedFaces[0].FaceAttributes.Emotion.Happiness.ToString("0.##");
            SadnessValue.Text = (DetectedFaces[0].FaceAttributes.Emotion.Sadness + (DetectedFaces[0].FaceAttributes.Emotion.Neutral / 2)).ToString("0.##");


        }
        private async void button1_Click(object sender, EventArgs e)
        {





            //This Shit is working !!!
            //_ = await faceClient.Face.DetectWithUrlAsync(imageUrl, false, false, recognitionModel: "recognition_01", returnFaceAttributes: requiredFaceAttributes);
            using (FolderBrowserDialog? fbd = new())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    files = Directory.GetFiles(fbd.SelectedPath);

                }

                await UpdateUIWithFaceFeatures(faceClient, files[CurrentImage]);
            }


            //OpenFileDialog openFileDialog1 = new OpenFileDialog
            //{
            //    Title = "Browse Text Files",

            //    CheckFileExists = true,
            //    CheckPathExists = true,

            //    DefaultExt = "jpg",
            //    Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;...",

            //    FilterIndex = 2,
            //    RestoreDirectory = true,

            //    ReadOnlyChecked = true,
            //    ShowReadOnly = true
            //};

            //if (openFileDialog1.ShowDialog() == DialogResult.OK)
            //{
            //    pictureBox1.Image = new Bitmap (openFileDialog1.FileName);
            //}
            // string LocalImage = "C:\\Netways\\Presentation\\MahmoudElnagar.png";




            // var faces2 = await faceClient.Face.DetectWithUrlAsync("https://source.unsplash.com/pQV8dGHrOLU", returnFaceLandmarks:true,recognitionModel: "recognition_03", detectionModel: DetectionModel.Detection02);
            Console.WriteLine("");
        }

        private async void Next_Click(object sender, EventArgs e)
        {
            //    pictureBox1.Image = new Bitmap (openFileDialog1.FileName);

            if (CurrentImage + 1 >= files.Length)
            {
                CurrentImage = 0;
            }
            else
            {
                CurrentImage++;

            }
            await UpdateUIWithFaceFeatures(faceClient, files[CurrentImage]);

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private async void Previous_Click(object sender, EventArgs e)
        {
            if (CurrentImage - 1 < 0)
            {
                CurrentImage = files.Length - 1;
            }
            else
            {
                CurrentImage--;

            }
            await UpdateUIWithFaceFeatures(faceClient, files[CurrentImage]);

            await UpdateUIWithFaceFeatures(faceClient, files[CurrentImage]);

        }
    }
}

