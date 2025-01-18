namespace ConsoleToolKit;

public class TicTacToe
{
    // ToDo: Die Schriftfarbe der Meldungen ändern sich, sobald in C3 ein Wert gesetzt wurde.
    // Public Eigenschaften
    public ConsoleColor FehlerMeldung { get; set; }
    public ConsoleColor Player1Farbe { get; set; }
    public ConsoleColor Player2Farbe { get; set; }
    public ConsoleColor TitelHgFarbe { get; set; }
    public ConsoleColor TitelFgFarbe { get; set; }
    public ConsoleColor GewonnenFarbe { get; set; }
    public string Player1Zeichen { get; set; }
    public string Player2Zeichen { get; set; }
    public string LeeresFeld { get; set; }
    
    // Private Eigenschaften
    private string[,] Spielfeld { get; set; } 
    private string Player1 { get; set; }
    private string Player2 { get; set; }
    private List<string> Buchstaben { get; set; }
    private List<string> Zahlen { get; set; }
    //private string[] AktuelleUserEingabe { get; set; }
    private Dictionary<string, int> AktuelleKoordinaten { get; set; }

    /// <summary>
    /// In dem Konstruktor werden alle notwendigen Eigenschaften gesetzt.
    /// </summary>
    public TicTacToe()
    {
        // Danach werden alle Variablen vorbereitet.
        Player1 = "Player1";
        Player2 = "Player2";
        Player1Zeichen = "X";
        Player2Zeichen = "O";
        LeeresFeld = "#";

        AktuelleKoordinaten = new Dictionary<string, int>();
        Spielfeld = new string[4,4]
        {
            { " ", "1", "2", "3" },
            { "A", LeeresFeld, LeeresFeld, LeeresFeld },
            { "B", LeeresFeld, LeeresFeld, LeeresFeld },
            { "C", LeeresFeld, LeeresFeld, LeeresFeld }
        };

        Buchstaben = new List<string> { "A", "B", "C" };
        Zahlen = new List<string> { "1", "2", "3" };
        
        AktuelleKoordinaten.Add("vertikal", 0);
        AktuelleKoordinaten.Add("horizontal", 0);

        FehlerMeldung = ConsoleColor.Red;
        Player1Farbe = ConsoleColor.DarkRed;
        Player2Farbe = ConsoleColor.DarkYellow;
        TitelHgFarbe = ConsoleColor.Green;
        TitelFgFarbe = ConsoleColor.DarkBlue;
        GewonnenFarbe = ConsoleColor.Magenta;
    }

    /// <summary>
    /// In dieser Methode startet das Spiel.
    /// </summary>
    public void SpielStarten()
    {
        // In diesen Variablen werden alle Infos des aktiven Benutzer gespeichert.
        var spielerNr = 1;
        var aktuellerSpieler = "";
        var aktuellesZeichen = "";
        ConsoleColor aktuelleFarbe = ConsoleColor.Black;
        
        // Zuerst werden die beiden Spielernamen abgefragt.
        SpielerInitialisieren();
        
        // Nun wird das Spielfeld angezeigt.
        SpielfeldZeichnen();

        // In dieser Schleife werden die einzelnen Spielzüge abgefragt.
        while (true)
        {
            // Hier wird, je nach aktuellem Spieler, die entsprechenden Infos ermittelt.
            switch (spielerNr)
            {
                case 1:
                    aktuellerSpieler = Player1;
                    aktuellesZeichen = Player1Zeichen;
                    aktuelleFarbe = Player1Farbe;
                    break;
                case 2:
                    aktuellerSpieler = Player2;
                    aktuellesZeichen = Player2Zeichen;
                    aktuelleFarbe = Player2Farbe;
                    break;
            }

            // Welcher Spieler an der Reihe ist, wird in der entsprechenden Farbe ausgegeben.
            Console.ForegroundColor = aktuelleFarbe;
            Console.WriteLine($"{aktuellerSpieler} ist mit dem Zeichen {aktuellesZeichen} dran.");
            Console.ResetColor();
            
            // Nun wird die Eingabe des Spielers eingeholt.
            var eingabe = Console.ReadLine();

            // Zuerst muss geprüft werden, ob die Eingabe überhaupt gültig ist.
            // Ist dem nicht so, wird die while Schleife direkt wiederholt.
            if (IstEingabeGueltig(eingabe))
            {
                // Als Nächstes muss geprüft werden, ob das Feld bereits belegt ist.
                if (!IstFeldBelegt())
                {
                    // In der Variable Spielfeld ist das aktuelle Spielfeld gespeichert.
                    // Die aktuelle Eingabe wird in der Variable (Directory) als vertikaler und horizontaler Wert gespeichert.
                    // Ist das Feld belegt, wird die while Schleife neu gestartet.
                    Spielfeld[AktuelleKoordinaten["vertikal"], AktuelleKoordinaten["horizontal"]] = aktuellesZeichen;
                }
                else
                {
                    // Ist das Spielfeld bereits belegt, wird die while-Schleife erneut durchlaufen.
                    // So wird nochmals nach einer Eingabe gefragt.
                    continue;
                }
            }
            else
            {
                // Auch wenn die Eingabe ungültig ist, wird die while-Schleife erneut durchlaufen.
                continue;
            }

            // Nach dem Aktualisieren des Spielfelds wird dieses neu ausgegeben.
            SpielfeldZeichnen();
            
            // Nach der Eingabe muss zuerst geprüft werden, ob nun ein Spieler gewonnen hat.
            if (HatJemandGewonnen(spielerNr))
            {
                // Hat ein Spieler gewonnen, wird ein Text mit Linien ausgegeben.
                GewinnerBekanntgeben(aktuellerSpieler);
                break;
            }
            
            // Ob das Spielfeld voll ist, darf erst nach der Prüfung, ob jemand gewonnen hat stattfinden.
            // Hat ein benutzer mit dem letzten möglichen Spielzug gewonnen, würde ansonsten das Volle Feld das Spiel
            // Beenden.
            if (IstSpielfeldVoll())
            {
                Console.WriteLine("Unentschieden, Spielfeld ist voll.");
                break;
            }

            // Hier wird der aktuelle Spieler gewechselt.
            if (spielerNr == 1)
                spielerNr = 2;
            else
                spielerNr = 1;
        }
    }
    
    /// <summary>
    /// Prüft, ob es auf dem Spielfeld noch freie Plätze gibt.
    /// </summary>
    /// <returns>Gibt es noch freie Plätze, wird true zurückgegeben, ansonsten false.</returns>
    private bool IstSpielfeldVoll()
    {
        // Durchläuft die vertikalen Spalten. 
        for (int vertikal = 1; vertikal < 4; vertikal++)
        {
            // Durchläuft die horizontale Reihen.
            for (int horizontal = 1; horizontal < 4; horizontal++)
            {
                if (Spielfeld[vertikal, horizontal] == LeeresFeld)
                {
                    // Gibt es noch ein leeres Feld, ist das Spielfeld noch nicht voll.
                    return false;
                }
            }
        }

        return true;
    }
    
    /// <summary>
    /// In dieser Methode wird geprüft, ob das Feld, welches über die Koordinaten angegeben wurde, überhaupt noch frei ist.
    /// </summary>
    /// <param name="koordinaten">Ein Array </param>
    /// <returns></returns>
    private bool IstFeldBelegt()
    {
        // int vertikal = Buchstaben.IndexOf(koordinaten[0]) + 1;
        // int horizontal = int.Parse(koordinaten[1]);
        
        if (Spielfeld[AktuelleKoordinaten["vertikal"], AktuelleKoordinaten["horizontal"]] != LeeresFeld)
        {
            Console.WriteLine("Das Feld ist bereits belegt.");
            return true;
        }

        return false;
    }

    /// <summary>
    /// In dieser Methode wird geprüft, ob der aktive Spieler gewonnen hat.
    /// </summary>
    /// <param name="playerNr">1 für den Player1 und 2 für den Player2.</param>
    /// <returns>Hat der Spieler gewonnen wird ture zurückgegeben, andernfalls false.</returns>
    private bool HatJemandGewonnen(int playerNr)
    {
        var anzahlFelderVertikaler = 0;
        var anzahlFelderHorizontal = 0;
        string aktuellesPlayerZeichen = "";

        // Hier wird das Zeichen des entsprechenden Players ermittelt.
        if (playerNr == 1)
            aktuellesPlayerZeichen = Player1Zeichen;
        if (playerNr == 2)
            aktuellesPlayerZeichen = Player2Zeichen;
        
        // In zwei Schleifen wird das ganze Spielfeld durchlaufen und der Inhalt wird geprüft.
        for (int vertikal = 1; vertikal < 4; vertikal++)
        {
            for (int horizontal = 1; horizontal < 4; horizontal++)
            {
                // Bei jedem Durchlauf werden die vertikalen (Spalten) und ...
                if (Spielfeld[vertikal, horizontal] == aktuellesPlayerZeichen)
                {
                    anzahlFelderVertikaler++;
                }
                // ... die horizontalen (Zeilen) auf die Zeichen des aktiven Spielers geprüft.
                if (Spielfeld[horizontal, vertikal] == aktuellesPlayerZeichen)
                {
                    anzahlFelderHorizontal++;
                }
            }
            // Gibt es, vertikal oder horizontal, 3 in einer Reihe, hat der aktuelle Spieler gewonnen.
            if (anzahlFelderVertikaler == 3 || anzahlFelderHorizontal == 3)
            {
                return true;
            }
            else
            {
                anzahlFelderVertikaler = 0;
                anzahlFelderHorizontal = 0;
            }
        }

        
        
        // Die Diagonale Felder dürfen dabei nicht vergessen werden.
        if (Spielfeld[1,1] == aktuellesPlayerZeichen && Spielfeld[2,2] == aktuellesPlayerZeichen && Spielfeld[3,3] == aktuellesPlayerZeichen)
        {
            return true;
        }
        if (Spielfeld[1,3] == aktuellesPlayerZeichen && Spielfeld[2,2] == aktuellesPlayerZeichen && Spielfeld[3,1] == aktuellesPlayerZeichen)
        {
            return true;
        }
        
        return false;
    }
    
    /// <summary>
    /// In dieser Methode wird geprüft, ob die Koordinaten, die der Benutzer eingegeben hat, korrekt sind. 
    /// </summary>
    /// <param name="eingabe">Die Koordinaten-Eingabe des Benutzers.</param>
    /// <returns>Wurden korrekte Koordinaten eingegeben, wird true zurückgegeben, andernfalls false.</returns>
    private bool IstEingabeGueltig(string eingabe)
    {
        // Das Array, in welchem die Koordinaten aufgesplitet werden.
        string[] aufgeteilteEingabe = new string[2];
        
        // Die Eingabe darf nicht länger 3 Zeichen sein. 
        if (eingabe.Length > 3)
        {
            Console.WriteLine("Hier wurden zu viele Zeichen eingegeben.");
            return false;
        }

        // Gibt es ein Komma in der Eingabe, wird diese in ein Array geteilt.
        if (eingabe.IndexOf(",") != -1)
        {
            aufgeteilteEingabe = eingabe.Split(",");
        }

        // Besteht die Eingabe aus nur 2 Zeichen, werden die beiden ebenfalls in ein Array aufgeteilt.
        if (eingabe.Length == 2)
        {
            aufgeteilteEingabe[0] = eingabe[0].ToString();
            aufgeteilteEingabe[1] = eingabe[1].ToString();
        }

        // Der Buchstabe wird als Grossbuchstabe gespeichert.
        aufgeteilteEingabe[0] = aufgeteilteEingabe[0].ToUpper();

        // Der erste Wert der Koordinate muss ein Buchstabe sein.
        if (Buchstaben.Contains(aufgeteilteEingabe[0]) == false)
        {
            Console.WriteLine("Die erste Koordinate muss ein Buchstaben sein.");
            return false;
        }
        
        // Hier wird die vertikale Koordinate im 2-D Array gespeichert. 
        // Würde hier '.Add("vertikal", Buchstaben.IndexOf(aufgeteilteEingabe[0] + 1)) stehen,
        // würde -1 gespeichert werden.
        AktuelleKoordinaten["vertikal"] = Buchstaben.IndexOf(aufgeteilteEingabe[0]) + 1;

        // Der zweite Wert der Koordinate muss eine Zahl sein.
        if (Zahlen.Contains(aufgeteilteEingabe[1]) == false)
        {
            Console.WriteLine("Die zweite Koordinate muss eine Zahl sein.");
            return false;
        }

        AktuelleKoordinaten["horizontal"] = Zahlen.IndexOf(aufgeteilteEingabe[1]) + 1;
        
        // Läuft alles bis hier her durch, wird true zurückgegeben.
        return true;
    }
    
    /// <summary>
    /// Zeigt das Spielfeld an.
    /// </summary>
    private void SpielfeldZeichnen()
    {
        // Diese Schleife durchläuft den Horizontalen Teil des Arrays. 
        for (int vertikal = 0; vertikal < 4; vertikal++)
        {
            // Hier wird der Vertikale Teil des Arrays durchlaufen.
            for (int horizontal = 0; horizontal < 4; horizontal++)
            {
                Console.ResetColor();
                
                // Alle Felder die zum Titel gehören, werden in einer anderen Farbe dargestellt.
                if (Buchstaben.Contains(Spielfeld[vertikal, horizontal]) || Zahlen.Contains(Spielfeld[vertikal, horizontal]) || Spielfeld[vertikal, horizontal] == " ")
                {
                    Console.ForegroundColor = TitelFgFarbe;
                    Console.BackgroundColor = TitelHgFarbe;
                }

                if (Spielfeld[vertikal, horizontal] == Player1Zeichen)
                {
                    Console.ForegroundColor = Player1Farbe;
                }

                if (Spielfeld[vertikal, horizontal] == Player2Zeichen)
                {
                    Console.ForegroundColor = Player2Farbe;
                }
                
                // Der Inhalt jedes Feldes wird ausgegeben.
                Console.Write($"{Spielfeld[vertikal,horizontal]} ");
            }

            // Nach jeder Linie kommt eine neue Zeile.
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Hier wird der Name des Gewinners hübsch ausgegeben.
    /// </summary>
    /// <param name="benutzername">Der Name des Gewinners.</param>
    private void GewinnerBekanntgeben(string benutzername)
    {
        // Hier wird der Text generiert, der ausgegeben werden soll.
        var Siegertext = benutzername + " hat gewonnen.";
        var Linie = "";

        // Der Text soll oben wie unten mit Sonderzeichen unterstrichen werden. Damit diese exakt so Lang sind
        // wie der Text, wird hier ein String aufgebaut, der sich an der Textlänge orientiert.
        for (int i = 0; i < Siegertext.Length; i++)
        {
            Linie += "-";
        }
        
        // Der Gewinnertext wird in einer bestimmten Farbe ausgegeben.
        Console.ForegroundColor = GewonnenFarbe;
        // Hier wird nun die Sonderzeichen-Linie über und unter dem Text ausgegeben.
        Console.WriteLine(Linie);
        Console.WriteLine(Siegertext);
        Console.WriteLine(Linie);
        Console.ResetColor();
    }
    
    /// <summary>
    /// Ermittelt die Namen für 2 Spieler. 
    /// </summary>
    private void SpielerInitialisieren()
    {
        // Beide Spielernamen sollen hier direkt ermittelt werden.
        for (int i = 0; i < 2; i++)
        {
            // Stellt sicher, dass der Name so lange abgefragt wird, bis dieser den Regeln entspricht.
            bool eingabeOk = false;

            while (!eingabeOk)
            {
                // Fragt den Namen ab und speichert die Eingabe.
                Console.WriteLine($"Wie soll der Spieler{i + 1} heissen?");
                var eingabe = Console.ReadLine();

                // Die Schriftfarbe kann bei Fehlermeldungen entsprechend eingestellt werden.
                Console.ForegroundColor = FehlerMeldung;

                // Der Name muss aus mindestens 2 Zeichen bestehen.
                if (eingabe.Length < 2)
                {
                    Console.WriteLine("Der Name muss aus mindestens 2 Zeichen bestehen.");
                    // Continue ist notwendig, damit die while Schleife
                    continue;
                }

                // Das erste Zeichen darf keine Zahl sein.
                // Über TryParse wird aus dem ersten Buchstaben der Eingabe versucht eine Zahl auszulesen.
                // Gelingt dies, ist das erste Zeichen eine Zahl, was nicht erlaubt ist.
                if (int.TryParse(eingabe[0].ToString(), out int zahl))
                {
                    Console.WriteLine("Der Name muss mit einem Buchstaben beginnen.");
                    continue;
                }

                // Der normale Text wird in den Standard-Farben wiedergegeben.
                Console.ResetColor();
                
                // Beim Index 0 wird der erste Player mit dem eingegebenen Namen versehen, beim Index 1
                // ist es der 2. Player.
                if (i == 0)
                {
                    Player1 = eingabe;
                }
                else
                {
                    Player2 = eingabe;
                }

                // Läuft der Code bis hier hin, war der Benutzername korrekt.
                eingabeOk = true;
            }
        }
    }
}