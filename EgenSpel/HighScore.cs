﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MySql.Data;
using MySql.Data.MySqlClient;

// =======================================================================
// HsItem, en behållare-klass som innehåller info om en person i
// highscorelistan.
// =======================================================================
class HSItem
{
    // Variabler och egenskaper för dem:
    string name;
    public int points;

    public string Name { get { return name; } set { name = value; } }

    public int Points { get { return points; } set { points = value; } }

    // =======================================================================
    // HSItem(), klassens konstruktor
    // =======================================================================
    public HSItem(string name, int points)
    {
        this.name = name;
        this.points = points;
    }
}

// =======================================================================
// HighScore, innehåller en lista med hsItems samt metoder för att
// manipulera listan.
// =======================================================================
class HighScore
{
    SpriteFont font;
    int maxInList = 10; // Hur många som får vara i listan
    List<HSItem> highscore = new List<HSItem>();
    string name; // Spelarens namn

    // Används för att skriva ut vilket tecken spelaren har valt just nu:
    string currentChar;
    int key_index = 0; // Denna används för att mata in spelarens namn
                       // Dessa används för att kontrollera när tangenter trycktes in:
    double lastChange = 0;
    Keys previousKey;

    // =======================================================================
    // HighScore(), klassens konstruktor
    // =======================================================================
    public HighScore(int maxInList, SpriteFont font)
    {
        this.maxInList = maxInList;
        this.font = font;
    }

    // =======================================================================
    // Sort(),  metod som sorterar listan. Metoden
    // anropas av Add() när en ny person läggs till i
    // listan. Använder algoritmen bubblesort
    // =======================================================================
    void Sort()
    {
        int max = highscore.Count - 1;

        // Den yttre loopen, går igenom hela listan            
        for (int i = 0; i < max; i++)
        {
            // Den inre, går igenom element för element
            int nrLeft = max - i; // För att se hur många som redan gåtts igenom
            for (int j = 0; j < nrLeft; j++)
            {
                if (highscore[j].Points < highscore[j + 1].Points) // Jämför elementen
                {
                    // Byt plats!
                    HSItem temp = highscore[j];
                    highscore[j] = highscore[j + 1];
                    highscore[j + 1] = temp;
                }
            }
        }
    }

    // =======================================================================
    // Add(), lägger till en person i highscore-listan.
    // =======================================================================
    void Add(int points)
    {
        // Skapa en temporär variabel av typen HSItem:
        HSItem temp = new HSItem(name, points);
        // Lägg till tmp i listan. Observera att följande Add()
        // tillhör klassen List (är alltså skapad av Microsoft).
        // Metoden har endast samma namn, som just denna Add():
        highscore.Add(temp);
        Sort(); // Sortera listan efter att vi har lagt till en person!

        // Är det för många i listan?
        if (highscore.Count > maxInList)
        {
            // Eftersom vi har lagt till endast en person nu, så betyder
            // det att det är en person för mycket. Index på personen
            // som är sist i listan, är samma som maxInList. Vi vill ju
            // att det högsta indexet ska vara maxInList-1. Allstå kan
            // vi bara ta bort elementet med index maxInList.
            // Exempel:
            // maxInList är 5, vi har 6 element i listan. Det sjätte
            // elementet har index 5. Vi gör highscore.RemoveAt(5):
            highscore.RemoveAt(maxInList);
        }
    }

    // =======================================================================
    // CheckKey(), kontrollerar om en viss tangent har tryckts och huruvida
    // det har gått lagomt lång tid (130ms) sedan tidigare tryck av samma
    // tangent.
    // =======================================================================
    bool CheckKey(Keys key, GameTime gameTime)
    {
        KeyboardState keyboardState = Keyboard.GetState();
        if (keyboardState.IsKeyDown(key))
        {
            // Har det gått lagomt lång tid, eller är det en helt annan
            // tangent som trycks ned denna gång?
            if (lastChange + 130 < gameTime.TotalGameTime.TotalMilliseconds
                || previousKey != key)
            {
                // sätt om variablerna inför nästa varv i spelloopen:
                previousKey = key;
                lastChange = gameTime.TotalGameTime.TotalMilliseconds;
                return true;
            }
        }
        // Just den tangenten (key) trycktes INTE ned, eller så trycktes den
        // ned alldeles nyligen (mindre än 130ms):
        return false;
    }

    // =======================================================================
    // PrintDraw(), metod för att skriva ut listan. Det finns ingen
    // PrintUpdate() då det är en helt statisk text som skrivs ut.
    // =======================================================================
    public void PrintDraw(SpriteBatch spriteBatch/*, SpriteFont font*/)
    {

        string text = "HIGHSCORE (Press B to return to Menu)\n";
        foreach (HSItem h in highscore)
            text += h.Name + " " + h.Points + "\n";

        spriteBatch.DrawString(font, text, Vector2.Zero, Color.White);
    }

    // =======================================================================
    // EnterUpdate(), här matar användaren in sitt användarnamn. Precis som
    // klassiska gamla arkadspel kan man ha tre tecken A-Z i sitt namn. Detta
    // är Update-delen i spel-loopen för inmatning av highscore-namn. Metoden
    // ska fortsätta anropas av Update() så länge true returneras.
    // =======================================================================
    public bool EnterUpdate(GameTime gameTime, int points)
    {
        // Vilka tecken som är möjliga:
        char[] key = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K',
                       'L', 'M', 'N', 'O', 'P',  'Q', 'R', 'S', 'T', 'U',
                       'V','W', 'X', 'Y', 'Z'};


        // Användaren trycker knappen nedåt, stega framlänges i key-vektorn:
        if (CheckKey(Keys.Down, gameTime))
        {
            key_index++;
            if (key_index >= key.Length)
                key_index = 0;
        }

        // Användaren trycker knappen uppåt, stega baklänges i key-vektorn:
        if (CheckKey(Keys.Up, gameTime))
        {
            key_index--;
            if (key_index <= 0)
                key_index = key.Length - 1;
        }

        // Användaren trycker ENTER, lägg till det valda tecknet i 
        if (CheckKey(Keys.Enter, gameTime))
        {
            name += key[key_index].ToString();
            if (name.Length == 3)
            {
                // Återställ namnet och allt så att man kan lägga till namnet 
                // på en ny spelare:
                Add(points);
                name = "";
                currentChar = "";
                key_index = 0;
                return true; // Ange att vi är klara
            }
        }
        // Lagra det tecken som nu är valt, så att vi kan skriva ut det i
        // EnterDraw():




        currentChar = key[key_index].ToString();
        // Ange att vi inte är klara, fortsätt anropa denna metod via Update(): 
        return false;
    }



    // =======================================================================
    // EnterDraw(), skriver ut de tecken spelaren har matat in av sitt namn
    // (om något) samt det tecken (av tre) som just nu är valt.
    // =======================================================================
    public void EnterDraw(SpriteBatch spriteBatch/*, SpriteFont font*/)
    {
        string text = "ENTER NAME:" + name + currentChar;
        spriteBatch.DrawString(font, text, Vector2.Zero, Color.White);
    }
    // =======================================================================
    // SaveToFile(), spara till fil.
    // =======================================================================
    public void SaveToFile(string filename)
    {

        StreamWriter sw = new StreamWriter(filename);
        foreach (HSItem item in highscore)
        {
            string text = item.Name + ":" + item.Points;
            sw.WriteLine(text);
        }
        sw.Close();
    }

    // =======================================================================
    // SaveToFile(), spara till fil.
    // =======================================================================
    public void SaveToDB() //TL-190203 Nu använder ni inte databasen. Testa först att lyckas med filsparningen. Testa sedan att fixa databasen.
    {
        string connStr = "server=185.189.48.15; user=prg_user; database=prg_db; port=3306; password=wCoLyemcmI";
        MySqlConnection conn = new MySqlConnection(connStr);

        try
        {

            conn.Open();
            string strGame = "t-space"; //TL-190203 Byt ut namnet t-space till ert eget namn.

            foreach (HSItem item in highscore)
            {
                string sql = "INSERT INTO scores (name, score,game) VALUES ('" + item.Name + "','" + item.Points + "','" + strGame + "')";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();

            }



        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        conn.Close();


    }
    // =======================================================================
    // LoadFromFile(), ladda från fil.
    // =======================================================================
    public void LoadFromFile(string filename)
    {
        StreamReader sr = new StreamReader(filename);
        string row;
        while ((row = sr.ReadLine()) != null)
        {
            string[] words = row.Split(':');
            int points = Convert.ToInt32(words[1]);
            HSItem temp = new HSItem(words[0], points);
            highscore.Add(temp);
        }
        sr.Close();
    }

    public void LoadFromDB()
    {
        string connStr = "server=185.189.48.15; user=prg_user; database=prg_db; port=3306; password=wCoLyemcmI";
        MySqlConnection conn = new MySqlConnection(connStr);

        try
        {
            conn.Open();

            string sql = "SELECT name, score FROM scores WHERE game='t-space'"; //TL-190203 Byt ut namnet t-space till ert eget namn.
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Console.WriteLine(rdr[0] + "--" + rdr[1]); //TL-190203 Ni har ingen console att skriva i. Använd istället följande två rader.
                //HSItem temp = new HSItem(rdr[0].ToString(), int.Parse(rdr[1].ToString()));
                //highscore.Add(temp);
            }
            rdr.Close();
        }

        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        conn.Close();
    }
}