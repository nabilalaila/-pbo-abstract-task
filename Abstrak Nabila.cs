using System;
using System.Data;

class Program
{
    static void Main(string[] args)
    {
        BosRobot KepalaRobot = new BosRobot("Adudu", 500, 100, 50);
        RobotMiliter musuh = new RobotMiliter("Halodek", 300, 50, 40, 5);

        Console.WriteLine("Identitas Bos Robot");
        KepalaRobot.cetakInformasi();
        Console.WriteLine();
        Console.WriteLine("Identitas Robot Militer");
        musuh.cetakInformasi();
        Console.WriteLine();

        Console.WriteLine("Kemampuan Perbaikan");
        Perbaikan perbaikan = new Perbaikan();
        KepalaRobot.GunakanKemampuan(perbaikan);
        perbaikan.UpdateCooldown();
        Console.WriteLine();

        Console.WriteLine("Kemampuan Electric Shock");
        ElectricShock electricShock = new ElectricShock();
        KepalaRobot.GunakanKemampuan(electricShock);
        electricShock.UpdateCooldown();
        Console.WriteLine();

        Console.WriteLine("Kemampuan Plasma Cannon");
        PlasmaCannon plasmaCannon = new PlasmaCannon();
        musuh.GunakanKemampuan(plasmaCannon);
        plasmaCannon.UpdateCooldown();
        Console.WriteLine();

        Console.WriteLine("Kemampuan Supershield");
        SuperShield superShield = new SuperShield();
        KepalaRobot.GunakanKemampuan(superShield);
        superShield.UpdateCooldown();
        Console.WriteLine();

        Console.WriteLine("Serangan Bos Robot");
        KepalaRobot.Serang(musuh);
        Console.WriteLine();

        Console.WriteLine("Serangan Musuh");
        musuh.Serang(KepalaRobot);
        Console.WriteLine();

        Console.WriteLine("Serangan peluru beruntun");
        musuh.tembakBeruntun(KepalaRobot);
        Console.WriteLine();

        Console.WriteLine("Update terbaru Bos Robot");
        KepalaRobot.cetakInformasi();
        Console.WriteLine();

        Console.WriteLine("Update terbaru Musuh");
        musuh.cetakInformasi();
        Console.WriteLine();
        
        Console.WriteLine("Percobaan kemampuan saat cooldown");
        KepalaRobot.GunakanKemampuan(perbaikan);
        KepalaRobot.GunakanKemampuan(electricShock);
        musuh.GunakanKemampuan(plasmaCannon);
        KepalaRobot.GunakanKemampuan(superShield);
    }
}

public interface Ikemampuan
{
    void GunakanKemampuan(Robot penyerang, Robot target);
    int Cooldown { get; set; }
}

public abstract class Robot
{
    public string nama;
    public int energi, armor, serangan;

    public Robot(string Nama, int Energi, int Armor, int Serangan)
    {
        this.nama = Nama;
        this.energi = Energi;
        this.armor = Armor;
        this.serangan = Serangan;
    }
    public void Serang(Robot target)
    {
        if (energi >= serangan)
        {
            energi -= serangan;
            Console.WriteLine($"Robot {nama} menyerang. Energinya sekarang : {energi}");
        }
        else
        {
            Console.WriteLine("Energi tidak cukup. Penyerangan dibatalkan");
        }
    }

    public abstract void GunakanKemampuan(Ikemampuan kemampuan);

    public void cetakInformasi()
    {
        Console.WriteLine($"Nama Robot : {nama}\nEnergi : {energi}\nArmor : {armor}\nSerangan : {serangan}");
    }
}

public class BosRobot : Robot
{
    public BosRobot(string nama, int energi, int armor, int serangan)
        : base(nama, energi, armor, serangan)
    { 
    }

    public override void GunakanKemampuan(Ikemampuan kemampuan)
    {
        kemampuan.GunakanKemampuan(this, this);
    }
    public void diserang(Robot penyerang, Robot target)
    {
        int kerusakan = penyerang.serangan - this.armor;
        if (kerusakan > 0)
        {
            energi -= kerusakan;
            Console.WriteLine($"Bos Robot {nama} diserang oleh {penyerang.nama}. Energi sekarang : {energi}\nEnergi musuh : {target.energi}");
        }
        else
        {
            Console.WriteLine($"Pertahanan Bos {nama} Robot tidak berhasil ditembus");
        }

        if (energi <= 0)
        {
            mati();
        }
    }
    public void mati()
    {
        Console.WriteLine($"Bos robot {nama} dah mati");
    }

}

public class Perbaikan : Ikemampuan
{
    public int Cooldown { get; set; }
    private int hitungCooldown; 

    public Perbaikan()
    {
        Cooldown = 3; 
        hitungCooldown = 0; 
    }

    public void GunakanKemampuan(Robot penyerang, Robot target)
    {
        if (hitungCooldown == 0)
        {
            penyerang.energi += 200;
            Console.WriteLine($"{penyerang.nama} dipulihkan. Energi sekarang : {penyerang.energi}");
            hitungCooldown = Cooldown;
        }
        else
        {
            Console.WriteLine("Kemampuan dalam cooldown.");
        }
    }

    public void UpdateCooldown()
    {
        if (Cooldown > 0)
           hitungCooldown--; 
    }
}

public class ElectricShock : Ikemampuan
{
    public int Cooldown { get; set; }
    private int hitungCooldown;

    public ElectricShock() 
    { 
        Cooldown = 4; hitungCooldown = 0;
    }

    public void GunakanKemampuan(Robot penyerang, Robot target)
    {
        if (hitungCooldown == 0 && penyerang.energi >= 30) 
        {
            penyerang.energi -= 30;
            target.energi -= 50;
            Console.WriteLine($"Robot {penyerang.nama} melakukan serangan listrik. Energi sekarang : {penyerang.energi}\nEnergi musuh : {target.energi}");
            hitungCooldown = Cooldown;
        }
        else
        {
            Console.WriteLine("Kekuatan tidak bisa digunakan. Isi energimu atau tunggu beberapa saat lagi");
        }
    }

    public void UpdateCooldown()
    {
        if (Cooldown > 0)
            hitungCooldown--;
    }   

}

public class PlasmaCannon : Ikemampuan
{
    public int Cooldown { get; set; }
    private int hitungCooldown;

    public PlasmaCannon() 
    { 
        Cooldown = 3; hitungCooldown = 0;
    }

    public void GunakanKemampuan(Robot penyerang, Robot target)
    {
        if (hitungCooldown == 0 && penyerang.energi >= 100)
        {
            target.armor -= 50;
            Console.WriteLine($"Robot {target.nama} melakukan serangan tembakan plasma. Energi Sekarag : {target.energi}\nEnergi musuh : {target.energi}");
            hitungCooldown = Cooldown;
        }
        else
        {
            Console.WriteLine("Kekuatan tidak bisa digunakan. Isi energimu atau tunggu beberapa saat lagi");
        }
    }
    public void UpdateCooldown()
    {
        if (hitungCooldown > 0)
            hitungCooldown--;
    }
}

public class SuperShield : Ikemampuan
{
    public int Cooldown { get; set; }
    private int hitungCooldown;

    public SuperShield() 
    { 
        Cooldown = 3; hitungCooldown=0;
    }

    public void GunakanKemampuan( Robot penyerang, Robot target)
    {
        penyerang.armor += 20;
        Console.WriteLine($"Armor telah ditingkatkan menjadi {penyerang.armor}");
        hitungCooldown = Cooldown;
    }

    public void UpdateCooldown()
    {
        if (hitungCooldown > 0)
            hitungCooldown--;
    }
}

public class RobotMiliter : Robot
{
    public int PeluruBeruntun;
    public RobotMiliter(string nama, int energi, int armor, int serangan, int peluruBeruntun)
        : base(nama, energi, armor, serangan)
    {
        this.PeluruBeruntun = peluruBeruntun;
    }

    public void tembakBeruntun(Robot target)
    {
        int PeluruBeruntun = 100;
        Console.Write("Peluru yang ditembakkan :");
        int JumlahPeluruTembak = int.Parse(Console.ReadLine());
        if (energi >= (2 * JumlahPeluruTembak) && (JumlahPeluruTembak<= PeluruBeruntun))
        {
            PeluruBeruntun -= JumlahPeluruTembak;
            energi -= (JumlahPeluruTembak * 2);
            Console.WriteLine($"Peluru beruntun ditembakkan. Sisa peluru : {PeluruBeruntun}. Energi : {energi}");
        }
        else
        {
            Console.WriteLine("Peluru tidak bisa ditembakkan. Energi atau peluru tidak mencukupi");
        }
    }

    public override void GunakanKemampuan(Ikemampuan kemampuan)
    {
        kemampuan.GunakanKemampuan(this, this);

    }
}