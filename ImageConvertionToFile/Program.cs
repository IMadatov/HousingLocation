


string[] array1 = Directory.GetFiles("C:\\ImageToDoProject"); 
Console.WriteLine("--- Files: ---");
var res= await ReadImage(array1[0]);
foreach (byte b in res)
{
    Console.Write(b);
}




async Task<byte[]> ReadImage(string path)
{
    return await File.ReadAllBytesAsync(path);
}
bool ByteArrayToFile(string fileName, byte[] byteArray)
{
    try
    {
        using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
        {
            fs.Write(byteArray, 0, byteArray.Length);
            return true;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Exception caught in process: {0}", ex);
        return false;
    }
}