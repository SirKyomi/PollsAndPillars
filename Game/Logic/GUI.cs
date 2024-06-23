 using Godot;
using System;

public partial class GUI : Control
{
    [Export]
    public GameStats gameStats;

    #region Stats

    private double _wohlstand;
    private double _arbeitslosigkeit;
    private double _klimabelastung;
    private double _waehlerzustimmung;
    
    //Ab dieser Zahl verliert der Spieler immer mehr Wählerzustimmung
    private const double Schwellenwert = 50;
    
    #endregion

    #region Timer & Zugehöriges

    private Timer _timer;
    private const double WaitTime = 0.05;
    private const int SekundenProWahlperiode = 15;

    private Timer _timerArbeitslosenHinzufuegen;
    private const int AnzahlAbreitslosenSteigerungenProWahlperiode = 8;
    private const double FaktorAbreitslosenSteigerungenProWahlperiode = 2;
    private const double WaitTimeArbeitslosenHinzufuegen = SekundenProWahlperiode / AnzahlAbreitslosenSteigerungenProWahlperiode;
    
    private double _vergangeneZeit = 0;
    
    #endregion
    
    

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SetzeStartWerte();
        InitialisiereTimer();
    }
    
    private void SetzeStartWerte()
    {
        _wohlstand = 50;
        _arbeitslosigkeit = 50;
        _klimabelastung = 50;
        _waehlerzustimmung = 100;
    }
    
    private void InitialisiereTimer()
    {
        _timer = new Timer() { WaitTime = WaitTime, Autostart = true, OneShot = false};
        _timer.Timeout += TimerOnTimeout;
        AddChild(_timer); 
        
        _timerArbeitslosenHinzufuegen = new Timer() { WaitTime = WaitTimeArbeitslosenHinzufuegen, Autostart = true, OneShot = false};
        _timerArbeitslosenHinzufuegen.Timeout += ErhoeheArbeitslosigkeit;
        AddChild(_timerArbeitslosenHinzufuegen);
    }

    private void TimerOnTimeout()
    {
        _vergangeneZeit += WaitTime;
        _vergangeneZeit = Math.Round(_vergangeneZeit, 4);
        var rest = (_vergangeneZeit % SekundenProWahlperiode);
        if (rest == 0 && _waehlerzustimmung < Schwellenwert)
            GameOver();

        var berechneteProzent = (rest / SekundenProWahlperiode) * 100;
        GetNode<ProgressBar>("WahlperiodeBar").Value = berechneteProzent;
    }

    private void ErhoeheArbeitslosigkeit()
    {
        ArbeitslosigkeitUiAktualisieren(FaktorAbreitslosenSteigerungenProWahlperiode);
        BerechneWaehlerzustimmung(_arbeitslosigkeit, FaktorAbreitslosenSteigerungenProWahlperiode, true, 30);

        if (_arbeitslosigkeit > 75) {
            BerechneWaehlerzustimmung(_arbeitslosigkeit, 10, true);
        }

        if (_klimabelastung > 75) {
            BerechneWaehlerzustimmung(_klimabelastung, 10, true);
        }
        
        if (_wohlstand < 25) {
            BerechneWaehlerzustimmung(_wohlstand, -10, false);
        }
    }

    private void ArbeitslosigkeitUiAktualisieren(double arbeitslosigkeit)
    {
        _arbeitslosigkeit += arbeitslosigkeit;
        _arbeitslosigkeit = Clamp0_100(_arbeitslosigkeit);
        GetNode<ProgressBar>("Panel/Arbeitslosigkeit/ArbeitslosigkeitBar").Value = _arbeitslosigkeit;
    } 
    private void WohlstandUiAktualisieren(double wohlstand)
    {
        _wohlstand += wohlstand;
        _wohlstand = Clamp0_100(_wohlstand);
        GetNode<ProgressBar>("Panel/Wohlstand/WohlstandBar").Value = _wohlstand;
    }  
    private void KlimabelastungUiAktualisieren(double klimabelastung)
    {
        _klimabelastung += klimabelastung;
        _klimabelastung = Clamp0_100(_klimabelastung);
        GetNode<ProgressBar>("Panel/Klimabelastung/KlimabelastungBar").Value = _klimabelastung;
    }
    private void WaehlerzustimmungUiAktualisieren()
    {
        _waehlerzustimmung = Clamp0_100(_waehlerzustimmung);
        GetNode<ProgressBar>("Waehlerzustimmung").Value = _waehlerzustimmung;
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        HandleGameOver();
    }

    private void HandleGameOver()
    {
        if (_waehlerzustimmung <= 0) GameOver();
    }

    private void GameOver() {
        gameStats.Score = _vergangeneZeit;
        gameStats.SekundenProWahlperiode = SekundenProWahlperiode;
        GetTree().ChangeSceneToFile("res://Scenes/GameOver.tscn");
    }

    public void OnPollsAndPillarsUiAktualisieren(double arbeitslosigkeit, double klimabelastung, double wohlstand) => WendeWerteAnUndBerechneWaehlerzustimmung(arbeitslosigkeit, klimabelastung, wohlstand);

    private void WendeWerteAnUndBerechneWaehlerzustimmung(double arbeitslosigkeit, double klimabelastung, double wohlstand)
    {
        KlimabelastungUiAktualisieren(klimabelastung);
        BerechneWaehlerzustimmung(_klimabelastung, klimabelastung, true, 30);

        WohlstandUiAktualisieren(wohlstand);
        BerechneWaehlerzustimmung(_wohlstand, wohlstand, false);

        
        ArbeitslosigkeitUiAktualisieren(arbeitslosigkeit);
        BerechneWaehlerzustimmung(_arbeitslosigkeit, arbeitslosigkeit, true, 30);
    }
    private void BerechneWaehlerzustimmung(double value, double changeValue, bool darfNichtHoeherSein, double schwellenwert = 50)
    {
        if (darfNichtHoeherSein && value > schwellenwert && changeValue > 0){ //minus
            _waehlerzustimmung -= changeValue * 0.66;
        }            
        else if (darfNichtHoeherSein && value < schwellenwert && changeValue < 0){
            _waehlerzustimmung += -(changeValue * 0.24); //plus
        }            
        if (!darfNichtHoeherSein && value < schwellenwert && changeValue < 0){
            _waehlerzustimmung -= -(changeValue * 0.66); //minus
        }            
        else if (!darfNichtHoeherSein && value > schwellenwert && changeValue > 0){
            _waehlerzustimmung += changeValue * 0.24; //plus
        }

        WaehlerzustimmungUiAktualisieren();
    }

    private static double Clamp0_100(double wert) => Math.Clamp(wert, 0, 100);
}