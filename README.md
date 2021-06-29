```csharp
string archiveFile = "./archive.bin";

        // Write
        FileWriter writer = new FileWriter();
		writer.SetPassword("abcd");
        writer.Open(archiveFile, true);
        string dir = Path.Combine(Application.dataPath, "XTC/Archive/Scripts");
        foreach(string file in Directory.GetFiles(dir))
        {
            byte[] data = File.ReadAllBytes(file);
            string filename = Path.GetFileName(file);
            writer.Write(filename, data);
        }
        writer.Flush();

        // Read

        FileReader reader = new FileReader();
        reader.Open(archiveFile);
        foreach(string entry in reader.entries)
        {
            Debug.Log(entry);
            byte[] data = reader.Read(entry);
            Debug.Log(System.Text.Encoding.UTF8.GetString(data));
        }

```

