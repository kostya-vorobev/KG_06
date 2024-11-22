using System;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Threading;

namespace KG_06
{
    public partial class MainWindow : Window
    {
        private BitmapSource _originalImage;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                _originalImage = new BitmapImage(new Uri(openFileDialog.FileName));
                ImageDisplay.Source = _originalImage;
                Console.WriteLine("Image Loaded.");
            }
        }
        private void ApplyExtendedGlassEffectButton_Click(object sender, RoutedEventArgs e)
        {
            // Применение эффекта, например, настройка фона или размытие
            // В WPF без использования дополнительных библиотек это можно сделать через стили и цвета
            // Например, создаем эффект размытия
            ImageDisplay.Effect = new System.Windows.Media.Effects.BlurEffect { Radius = 50 };

            // Также можно настроить дополнительные стили элементов интерфейса, чтобы имитировать эффекты стекла
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            // Сброс изображения на оригинал
            if (_originalImage != null)
            {
                ImageDisplay.Source = _originalImage;
            }
        }

        private void AddNoiseButton_Click(object sender, RoutedEventArgs e)
        {
            if (_originalImage != null)
            {
                ImageDisplay.Source = AddRandomNoise(_originalImage, 0.1); // 10% уровня шума
            }
        }

        private void AddPointNoiseButton_Click(object sender, RoutedEventArgs e)
        {
            if (_originalImage != null)
            {
                ImageDisplay.Source = AddPointNoise(_originalImage, 100); // 100 точек
            }
        }

        private void AddLineNoiseButton_Click(object sender, RoutedEventArgs e)
        {
            if (_originalImage != null)
            {
                ImageDisplay.Source = AddLineNoise(_originalImage, 5); // добавление 5 линий
            }
        }

        private void AddCircleNoiseButton_Click(object sender, RoutedEventArgs e)
        {
            if (_originalImage != null)
            {
                ImageDisplay.Source = AddCircleNoise(_originalImage, 5); // добавление 5 окружностей
            }
        }

        private BitmapSource AddRandomNoise(BitmapSource bitmap, double noiseLevel)
        {
            // Реализация добавления случайного шума
            int width = bitmap.PixelWidth;
            int height = bitmap.PixelHeight;
            int stride = width * (bitmap.Format.BitsPerPixel / 8);
            byte[] pixelData = new byte[height * stride];
            bitmap.CopyPixels(pixelData, stride, 0);

            Random rand = new Random();
            for (int i = 0; i < pixelData.Length; i += 4)
            {
                if (rand.NextDouble() < noiseLevel)
                {
                    pixelData[i] = (byte)rand.Next(0, 256);       // Blue
                    pixelData[i + 1] = (byte)rand.Next(0, 256);   // Green
                    pixelData[i + 2] = (byte)rand.Next(0, 256);   // Red
                }
            }

            return BitmapSource.Create(width, height, bitmap.DpiX, bitmap.DpiY, bitmap.Format, bitmap.Palette, pixelData, stride);
        }

        private BitmapSource AddPointNoise(BitmapSource bitmap, int pointCount)
        {
            // Добавление шумовых точек на изображение
            int width = bitmap.PixelWidth;
            int height = bitmap.PixelHeight;
            int stride = width * (bitmap.Format.BitsPerPixel / 8);
            byte[] pixelData = new byte[height * stride];
            bitmap.CopyPixels(pixelData, stride, 0);

            Random rand = new Random();
            for (int i = 0; i < pointCount; i++)
            {
                int x = rand.Next(width);
                int y = rand.Next(height);
                int index = (y * stride) + (x * 4);
                pixelData[index] = (byte)rand.Next(0, 256);          // Blue
                pixelData[index + 1] = (byte)rand.Next(0, 256);      // Green
                pixelData[index + 2] = (byte)rand.Next(0, 256);      // Red
            }

            return BitmapSource.Create(width, height, bitmap.DpiX, bitmap.DpiY, bitmap.Format, bitmap.Palette, pixelData, stride);
        }

        private BitmapSource AddLineNoise(BitmapSource bitmap, int lineCount)
        {
            // Добавление шумовых линий на изображение
            int width = bitmap.PixelWidth;
            int height = bitmap.PixelHeight;
            int stride = width * (bitmap.Format.BitsPerPixel / 8);
            byte[] pixelData = new byte[height * stride];
            bitmap.CopyPixels(pixelData, stride, 0);

            Random rand = new Random();
            for (int i = 0; i < lineCount; i++)
            {
                int startX = rand.Next(width);
                int endX = rand.Next(width);
                int y = rand.Next(height);
                for (int x = Math.Min(startX, endX); x <= Math.Max(startX, endX); x++)
                {
                    int index = (y * stride) + (x * 4);
                    pixelData[index] = (byte)rand.Next(0, 256);          // Blue
                    pixelData[index + 1] = (byte)rand.Next(0, 256);      // Green
                    pixelData[index + 2] = (byte)rand.Next(0, 256);      // Red
                }
            }

            return BitmapSource.Create(width, height, bitmap.DpiX, bitmap.DpiY, bitmap.Format, bitmap.Palette, pixelData, stride);
        }

        private BitmapSource AddCircleNoise(BitmapSource bitmap, int circleCount)
        {
            // Добавление шумовых окружностей на изображение
            int width = bitmap.PixelWidth;
            int height = bitmap.PixelHeight;
            int stride = width * (bitmap.Format.BitsPerPixel / 8);
            byte[] pixelData = new byte[height * stride];
            bitmap.CopyPixels(pixelData, stride, 0);

            Random rand = new Random();
            for (int i = 0; i < circleCount; i++)
            {
                int centerX = rand.Next(width);
                int centerY = rand.Next(height);
                int radius = rand.Next(5, 30); // Радиус окружности от 5 до 30
                for (int angle = 0; angle < 360; angle++)
                {
                    double rad = Math.PI * angle / 180;
                    int x = (int)(centerX + radius * Math.Cos(rad));
                    int y = (int)(centerY + radius * Math.Sin(rad));
                    if (x >= 0 && x < width && y >= 0 && y < height)
                    {
                        int index = (y * stride) + (x * 4);
                        pixelData[index] = (byte)rand.Next(0, 256);          // Blue
                        pixelData[index + 1] = (byte)rand.Next(0, 256);      // Green
                        pixelData[index + 2] = (byte)rand.Next(0, 256);      // Red
                    }
                }
            }

            return BitmapSource.Create(width, height, bitmap.DpiX, bitmap.DpiY, bitmap.Format, bitmap.Palette, pixelData, stride);
        }

        private void ApplyGaussianFilterButton_Click(object sender, RoutedEventArgs e)
        {
            if (_originalImage != null)
            {
                ImageDisplay.Source = ApplyGaussianBlur(_originalImage, 5); // Уровень размытия
            }
        }

        private void ApplyUniformFilterButton_Click(object sender, RoutedEventArgs e)
        {
            if (_originalImage != null)
            {
                ImageDisplay.Source = ApplyUniformBlur(_originalImage, 5); // Размер фильтра
            }
        }

        private void SharpenButton_Click(object sender, RoutedEventArgs e)
        {
            if (_originalImage != null)
            {
                ImageDisplay.Source = SharpenImage(_originalImage);
            }
        }

        private void ApplyGlassEffectButton_Click(object sender, RoutedEventArgs e)
        {
            if (_originalImage != null)
            {
                ImageDisplay.Source = ApplyGlassEffect(_originalImage, 5); // Уровень эффекта
            }
        }

        private BitmapSource AddNoise(BitmapSource bitmap, double noiseLevel)
        {
            int width = bitmap.PixelWidth;
            int height = bitmap.PixelHeight;
            int stride = width * (bitmap.Format.BitsPerPixel / 8);
            byte[] pixelData = new byte[height * stride];
            bitmap.CopyPixels(pixelData, stride, 0);

            Random rand = new Random();
            for (int i = 0; i < pixelData.Length; i += 4)
            {
                if (rand.NextDouble() < noiseLevel)
                {
                    // Добавление шума в одном из цветов
                    pixelData[i] = (byte)rand.Next(0, 256);       // Blue
                    pixelData[i + 1] = (byte)rand.Next(0, 256);   // Green
                    pixelData[i + 2] = (byte)rand.Next(0, 256);   // Red
                }
            }

            return BitmapSource.Create(width, height, bitmap.DpiX, bitmap.DpiY, bitmap.Format, bitmap.Palette, pixelData, stride);
        }

        private BitmapSource ApplyGaussianBlur(BitmapSource bitmap, int radius)
        {
            // Простая реализация фильтра Гаусса
            int kernelSize = radius * 2 + 1;
            double[,] kernel = CreateGaussianKernel(kernelSize, radius);
            return ApplyConvolution(bitmap, kernel);
        }

        private BitmapSource ApplyUniformBlur(BitmapSource bitmap, int radius)
        {
            // Простая реализация равномерного фильтра
            double[,] kernel = CreateUniformKernel(radius);
            return ApplyConvolution(bitmap, kernel);
        }

        private BitmapSource SharpenImage(BitmapSource bitmap)
        {
            double[,] kernel = new double[,]
            {
                { 0, -1, 0 },
                { -1, 5, -1 },
                { 0, -1, 0 }
            };
            return ApplyConvolution(bitmap, kernel);
        }

        private BitmapSource ApplyGlassEffect(BitmapSource bitmap, int ripple)
        {
            // Применение стеклянного эффекта
            int width = bitmap.PixelWidth;
            int height = bitmap.PixelHeight;
            int stride = width * (bitmap.Format.BitsPerPixel / 8);
            byte[] pixelData = new byte[height * stride];
            bitmap.CopyPixels(pixelData, stride, 0);

            var newPixelData = (byte[])pixelData.Clone();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int rippleOffset = (int)(Math.Sin(y * 0.1) * ripple);
                    int srcIndex = (y * stride) + (x * 4);
                    int dstIndex = (y * stride) + ((x + rippleOffset) * 4 + 4);

                    if (dstIndex < pixelData.Length - 4)
                    {
                        // Просто копируем цвет
                        newPixelData[dstIndex] = pixelData[srcIndex];       // Blue
                        newPixelData[dstIndex + 1] = pixelData[srcIndex + 1]; // Green
                        newPixelData[dstIndex + 2] = pixelData[srcIndex + 2]; // Red
                    }
                }
            }

            return BitmapSource.Create(width, height, bitmap.DpiX, bitmap.DpiY, bitmap.Format, bitmap.Palette, newPixelData, stride);
        }

        private BitmapSource ApplyConvolution(BitmapSource bitmap, double[,] kernel)
        {
            int width = bitmap.PixelWidth;
            int height = bitmap.PixelHeight;
            int stride = width * (bitmap.Format.BitsPerPixel / 8);
            byte[] pixelData = new byte[height * stride];
            bitmap.CopyPixels(pixelData, stride, 0);

            int kernelOffset = kernel.GetLength(0) / 2;
            int kernelSize = kernel.GetLength(0);
            var newPixelData = new byte[pixelData.Length];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    double blue = 0, green = 0, red = 0;

                    for (int ky = 0; ky < kernelSize; ky++)
                    {
                        for (int kx = 0; kx < kernelSize; kx++)
                        {
                            int pixelX = (x - kernelOffset + kx + width) % width;
                            int pixelY = (y - kernelOffset + ky + height) % height;

                            int srcIndex = (pixelY * stride) + (pixelX * 4);
                            blue += pixelData[srcIndex] * kernel[ky, kx];
                            green += pixelData[srcIndex + 1] * kernel[ky, kx];
                            red += pixelData[srcIndex + 2] * kernel[ky, kx];
                        }
                    }

                    int destIndex = (y * stride) + (x * 4);
                    newPixelData[destIndex] = Clamp((byte)blue);
                    newPixelData[destIndex + 1] = Clamp((byte)green);
                    newPixelData[destIndex + 2] = Clamp((byte)red);
                    newPixelData[destIndex + 3] = 255; // Alpha
                }
            }

            return BitmapSource.Create(width, height, bitmap.DpiX, bitmap.DpiY, bitmap.Format, bitmap.Palette, newPixelData, stride);
        }

        private double[,] CreateGaussianKernel(int size, int radius)
        {
            var kernel = new double[size, size];
            double sigma = radius / 3.0; // Параметры для гауссовой функции
            double sum = 0.0;
            int halfSize = size / 2;

            for (int x = -halfSize; x <= halfSize; x++)
            {
                for (int y = -halfSize; y <= halfSize; y++)
                {
                    double g = (1.0 / (2.0 * Math.PI * sigma * sigma)) *
                               Math.Exp(-(x * x + y * y) / (2 * sigma * sigma));
                    kernel[x + halfSize, y + halfSize] = g;
                    sum += g;
                }
            }

            // Нормализация ядра
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    kernel[i, j] /= sum;

            return kernel;
        }

        private double[,] CreateUniformKernel(int radius)
        {
            int size = radius * 2 + 1;
            var kernel = new double[size, size];
            double value = 1.0 / (size * size);
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    kernel[i, j] = value;
            return kernel;
        }

        private byte Clamp(byte value)
        {
            return (byte)Math.Max(0, Math.Min(255, (int)value));
        }
    }
}
