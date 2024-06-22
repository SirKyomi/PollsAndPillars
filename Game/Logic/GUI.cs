using Godot;
using System;

public partial class GUI : Control
{
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
    private const int SekundenProWahlperiode = 60;

    private Timer _timerArbeitslosenHinzufuegen;
    private const int AnzahlAbreitslosenSteigerungenProWahlperiode = 24;
    private const double FaktorAbreitslosenSteigerungenProWahlperiode = 4;
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
        var rest = (_vergangeneZeit % SekundenProWahlperiode);
        var berechneteProzent = (rest / SekundenProWahlperiode) * 100;
        GetNode<ProgressBar>("WahlperiodeBar").Value = berechneteProzent;
    }

    private void ErhoeheArbeitslosigkeit()
    {
        ArbeitslosigkeitUiAktualisieren(FaktorAbreitslosenSteigerungenProWahlperiode);
        BerechneWaehlerzustimmung(_arbeitslosigkeit, FaktorAbreitslosenSteigerungenProWahlperiode, true);
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
        if (_waehlerzustimmung <= 0) GetTree().ReloadCurrentScene();
    }

    public void OnPollsAndPillarsUiAktualisieren(double arbeitslosigkeit, double klimabelastung, double wohlstand) => WendeWerteAnUndBerechneWaehlerzustimmung(arbeitslosigkeit, klimabelastung, wohlstand);

    private void WendeWerteAnUndBerechneWaehlerzustimmung(double arbeitslosigkeit, double klimabelastung, double wohlstand)
    {
        KlimabelastungUiAktualisieren(klimabelastung);
        BerechneWaehlerzustimmung(_klimabelastung, klimabelastung, true);

        WohlstandUiAktualisieren(wohlstand);
        BerechneWaehlerzustimmung(_wohlstand, wohlstand, false);

        
        ArbeitslosigkeitUiAktualisieren(arbeitslosigkeit);
        BerechneWaehlerzustimmung(_arbeitslosigkeit, arbeitslosigkeit, true);
    }
    private void BerechneWaehlerzustimmung(double value, double changeValue, bool darfNichtHoeherSein)
    {
        if (darfNichtHoeherSein && value > Schwellenwert && changeValue > 0){
            _waehlerzustimmung -= changeValue * 0.33;
        }            
        else if (darfNichtHoeherSein && value < Schwellenwert && changeValue < 0){
            _waehlerzustimmung += -(changeValue * 0.33);
        }            
        if (!darfNichtHoeherSein && value < Schwellenwert && changeValue < 0){
            _waehlerzustimmung -= -(changeValue * 0.33);
        }            
        else if (!darfNichtHoeherSein && value > Schwellenwert && changeValue > 0){
            _waehlerzustimmung += changeValue * 0.33;
        }

        WaehlerzustimmungUiAktualisieren();
    }



    private static double Clamp0_100(double wert) => Math.Clamp(wert, 0, 100);
}