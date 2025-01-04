using System.Security.AccessControl;

namespace ConsoleToolKit;

public class Menu
{
    static void Main(string[] args)
    {
        var menuTitel = " Menü ";
        string[] menuEintraege =
        [
            "0 Anfang", "1 Start", "2 Einstellungen", "3 Informationen", " ", "8 Beenden", "9 Komm Du nur nach Hause",
            "10 Dachte ich mir doch das Du nicht willst"
        ];
        int frameHoehe;
        int frameBreite;
        const int frameAbstandOben = 1;
        const int frameAbstandUnten = 1;
        const int frameAbstandLinksRechts = 2;
        const int frameDicke = 1;
        const string frameZeichen = "#";
        const string frameEckeObenLinks = "\u23a1"; // ⦿
        const string frameEckeObenRechts = "\u23a4"; // ⦿
        const string frameEckeUntenLinks = "\u23a3"; // ⦿
        const string frameEckeUntenRechts = "\u23a6"; // ⦿
        const string frameHorizontalOben = "\u0305"; // ―
        const string frameHorizontalUnten = "\u0332"; // ―
        const string frameVertikalRechts = "┃"; // ┃
        const string frameVertikalLinks = "┃"; // ┃

        // ########## Frame-Breite ##########

        // Zuerst muss die Rahmengrösse ermittelt werden.
        // Dafür muss der längste Menüeintrag ermittelt werden.
        var laengsterMenuEintrag = 0;
        foreach (var eintrag in menuEintraege)
        {
            if (laengsterMenuEintrag < eintrag.Length)
            {
                laengsterMenuEintrag = eintrag.Length;
            }
        }

        // Links und rechts des längsten Menüeintrags sollen 2 Leerzeichen zum Rahmen vorhanden sein.
        // Zusätzlich muss auch die Rahmendicke (1 Zeichen) dazugerechnet werden.
        frameBreite = laengsterMenuEintrag + (2 * frameDicke) + (2 * frameAbstandLinksRechts);

        // ########## Frame-Höhe ##########

        // Die Rahmenhöhe berechnet sich aus der Anzahl der Menüeinträge. Oben gibt es zum Rahmen eine Zeile Abstand,
        // unten sind es 2 Zeilen distanz zum Rand.
        frameHoehe = menuEintraege.Length + (2 * frameDicke) + frameAbstandOben + frameAbstandUnten;

        // ########## Menü-Titel ##########

        // Der Menü-Titel darf nicht länger sein, als der Rahmen breit ist.
        if (menuTitel.Length > frameBreite)
        {
            // Ist der Titel länger als der Rahmen, wird ein Fehlermeldung in der Konsole ausgegeben.
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Fehler: Der Menü-Titel ist zu lang.");
            Console.WriteLine($"Dieser darf maximal {menuTitel.Length} Zeichen lang sein.");
            Console.ResetColor();
        }

        // Der Menü-Titel soll zentriert angezeigt werden.
        var leerzeichenBeiMenu = (frameBreite - menuTitel.Length) / 2;
        for (int i = 0; i < leerzeichenBeiMenu; i++)
        {
            Console.Write(" ");
        }

        Console.ForegroundColor = ConsoleColor.White;
        Console.BackgroundColor = ConsoleColor.Yellow;
        Console.Write(menuTitel);
        Console.ResetColor();
        Console.WriteLine();

        // ########## Menü-Rahmen ##########

        // Durchläuft alle Zahlen für die Rahmen-Höhe.
        for (int hoehe = 0; hoehe < frameHoehe; hoehe++)
        {
            // Durchläuft alle Zahlen für die Rahmen-Breite
            for (int breite = 0; breite < frameBreite; breite++)
            {
                // Damit die erste und die letzte Zeile komplett angezeigt werden, muss auf dies geprüft
                // werden. Da die Zählung bei 0 beginnt, muss von der frameHoehe 1 abgezogen werden.
                if (hoehe == 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;

                    // Die obere linke Ecke
                    if (breite == 0)
                    {
                        Console.Write(frameEckeObenLinks);
                    }
                    // Die obere rechte Ecke
                    else if (breite == frameBreite - 1)
                    {
                        Console.Write(frameEckeObenRechts);
                    }
                    // Alles zwischen den Ecken
                    else
                    {
                        Console.Write(frameHorizontalOben);
                    }

                    Console.ResetColor();
                }
                else if (hoehe == frameHoehe - 1)
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;

                    // Die untere rechte Ecke
                    if (breite == 0)
                    {
                        Console.Write(frameEckeUntenLinks);
                    }
                    // Die untere linke Ecke
                    else if (breite == frameBreite - 1)
                    {
                        Console.Write(frameEckeUntenRechts);
                    }
                    // Alles zwischen den Ecken
                    else
                    {
                        Console.Write(frameHorizontalUnten);
                    }

                    Console.ResetColor();
                }
                else
                {
                    // Hier muss nun noch zwischen den Menüeinträgen und den Leeren-Zeilen unterschieden werden.

                    // Befinden wir uns im Rahmen inneren, also nicht in der ersten oder letzten Zeile der Höhe,
                    // soll nur am Anfang und Ende das Zeichen ausgegeben werden. Der Rest wird mit Leerzeichen gefüllt.
                    if (breite == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.Write(frameVertikalLinks);
                        Console.ResetColor();
                    }
                    else if (breite == frameBreite - 1)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.Write(frameVertikalRechts);
                        Console.ResetColor();
                    }
                    else
                    {
                        // Zuerst muss errechnet werden, ab welcher Zähler-Nummer der untere Rand beginnt.
                        var anfangDesUnterenRandes = frameAbstandOben + menuEintraege.Length;

                        // Die Variable 'hoehe' enthält die vertikale Position im Menü. Die Rahmen-Dicke wird in diesen
                        // berechnungen nicht berücksichtigt. Zu dieser if-Anweisung gelangt das Programm erst ab dem
                        // 1. Index der Variable 'hoehe'. Entsprechend funktioniert dies hier. Die 'hoehe' als Index
                        // darf nicht grösser als der frameAbstandOben sein. Ist 'hoehe' 1, stimmt dies mit 
                        // frameAbstandOben, da dieser (in der Erstellungsphase) auch 1 ist.
                        // Die Variable 'anfangDesUnterenRandes' muss grösser als 'hoehe' sein, ansonsten wird der
                        // Wert 6 (in der Erstellungsphase) bereits zum Rand dazu gezählt.
                        if (hoehe <= frameAbstandOben || anfangDesUnterenRandes < hoehe)
                        {
                            Console.Write(" ");
                        }
                        else
                        {
                            // Hier wird nun der entsprechende Menüeintrag ins Menü eingebaut. Dafür müssen ein paar
                            // Werte ermittelt werden.
                            // Der Inhalt (Text) des entsprechenden Menüeintrags (Array). Der Index muss hier berechnet
                            // werden, damit mit den gleichen Werten (Index) gearbeitet werden kann. 
                            // In der Höhe muss auf 0 zurück gerechnet werden, damit der erste Wert aus dem Array 
                            // gelesen werden kann. 
                            var aktuellerMenuEintrag = menuEintraege[hoehe - frameDicke - frameAbstandOben];
                            // Der Menüeintrag soll in der Mitte stehen. Deshalb muss die Rahmenbreite und die Wortlänge
                            // voneinander abgezogen werden. Dieser Wert durch 2 ergibt den Startpunkt.
                            var startDesMenuEintrags = (frameBreite - aktuellerMenuEintrag.Length) / 2;
                            // Auch das Ende des Eintrags muss errechnet werden. Dies ist der Startpunkt plud
                            // die Länge des Menüeintrags.
                            var endeDesMenuEintrags =
                                startDesMenuEintrags + aktuellerMenuEintrag.Length;

                            // Sind wir beim Index 'breite' noch nicht beim Startpunkt oder beim Endpunkt, muss mit
                            // Leerzeichen aufgefüllt werden. 
                            if (breite < startDesMenuEintrags || breite >= endeDesMenuEintrags)
                            {
                                Console.Write(" ");
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                // Wurde der Startpunkt für den Menüeintrag erreicht, wird der Text des Menüeintrags
                                // in der Konsole ausgegeben.
                                Console.Write(aktuellerMenuEintrag);
                                // Nun muss die innere for-Schleife einen grösseren Sprung machen. Ansonsten würde
                                // der Menüeintrag als ein Zeichen in der Schleife berücksichtigt und den Rahmen sprengen.
                                // -1 muss gerechnet werden, da der Index 0 basierend ist.
                                breite += aktuellerMenuEintrag.Length - 1;
                                Console.ResetColor();
                            }
                        }
                    }
                }
            }

            Console.WriteLine();
        }

        var ungueltigeEingabe = true;
        var ausgewaehlterEintrag = "";
        int menuWahl;

        // In einer Schleife wird die Eingabe des Benutzers abgefragt.
        do
        {
            Console.Write("Triff Deine Wahl: ");
            var eingabe = Console.ReadLine();

            // Die Eingabe des Benutzers muss eine Zahl sein. 
            if (Int32.TryParse(eingabe, out menuWahl) == false)
            {
                Console.WriteLine("Es wurde keine Zahl eingegeben.");
            }
            else
            {
                for (int i = 0; i < menuEintraege.Length; i++)
                {
                    if (menuEintraege[i].IndexOf(eingabe) != -1)
                    {
                        ungueltigeEingabe = false;
                        ausgewaehlterEintrag = menuEintraege[i];
                        Console.WriteLine($"Position {i} im Menü");
                    }
                }
            }
        } while (ungueltigeEingabe);

        Console.WriteLine($"Gewählter Eintrag: {ausgewaehlterEintrag}");
    }
}