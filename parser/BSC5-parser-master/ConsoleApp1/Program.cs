using System;
using System.IO;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        string filePath = "BSC5ra";
        DateTime currentDate = DateTime.Now;
        DateTime referenceDate = new DateTime(1950, 1, 1);
        double yearsSince1950 = (currentDate - referenceDate).TotalDays / 365.25;

        using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            fs.Seek(28, SeekOrigin.Begin);
            int entrySize = 32;
            byte[] entryBuffer = new byte[entrySize];

            while (fs.Read(entryBuffer, 0, entrySize) == entrySize)
            {
                Entry entry = ProcessEntry(entryBuffer);

                Console.WriteLine($"Catalog Number: {entry.XNO}, RA: {entry.SRA0}, DEC: {entry.SDEC0}, Spectral Type: {entry.IS}, Magnitude: {(entry.MAG / 100.0)}, R.A. Proper Motion: {entry.XRPM}, Dec. Proper Motion: {entry.XDPM}");
                // Ajusta la posición por movimiento propio
                double adjustedRA = entry.SRA0 + (entry.XRPM * yearsSince1950);
                double adjustedDec = entry.SDEC0 + (entry.XDPM * yearsSince1950);

                Console.WriteLine($"Catalog Number: {entry.XNO}, Adjusted RA: {adjustedRA}, Adjusted DEC: {adjustedDec}, Spectral Type: {entry.IS}, Magnitude: {(entry.MAG / 100.0)}");
            }
        }
    }

    static Entry ProcessEntry(byte[] entryBuffer)
    {
        Entry entry = new Entry();
        entry.XNO = BitConverter.ToSingle(entryBuffer, 0);
        entry.SRA0 = BitConverter.ToDouble(entryBuffer, 4);
        entry.SDEC0 = BitConverter.ToDouble(entryBuffer, 12);
        entry.IS = Encoding.ASCII.GetString(entryBuffer, 20, 2).Trim();
        entry.MAG = BitConverter.ToInt16(entryBuffer, 22);
        entry.XRPM = BitConverter.ToSingle(entryBuffer, 24);
        entry.XDPM = BitConverter.ToSingle(entryBuffer, 28);
        return entry;
    }
}

class Entry
{
    public float XNO;
    public double SRA0;
    public double SDEC0;
    public string IS;
    public short MAG;
    public float XRPM;
    public float XDPM;
}
