using Godot;
using System;

public partial class GUI : Control
{
    private const double Schwellenwert = 50;
    private double _wohlstand;
    private double _arbeitslosigkeit;
    private double _klimabelastung;
    private double _waehlerzustimmung;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _wohlstand = 50;
        _arbeitslosigkeit = 50;
        _klimabelastung = 50;
        _waehlerzustimmung = 100;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void OnPollsAndPillarsUiAktualisieren(double wohlstand, double arbeitslosigkeit, double klimabelastung)
    {
        WendeWerteAnUndBerechneWaehlerzustimmung(wohlstand, arbeitslosigkeit, klimabelastung);
    }

    private void WendeWerteAnUndBerechneWaehlerzustimmung(double wohlstand, double arbeitslosigkeit,
        double klimabelastung)
    {
        if (klimabelastung >= 0)
            _klimabelastung += klimabelastung;
        else
            _klimabelastung -= klimabelastung * -1;
        if (wohlstand >= 0)
            _wohlstand += wohlstand;
        else
            _wohlstand -= wohlstand * -1;
        if (arbeitslosigkeit >= 0)
            _arbeitslosigkeit += arbeitslosigkeit;
        else
            _arbeitslosigkeit -= arbeitslosigkeit * -1;

        _arbeitslosigkeit = Clamp0_100(_arbeitslosigkeit);
        _wohlstand = Clamp0_100(_wohlstand);
        _klimabelastung = Clamp0_100(_klimabelastung);


        GetNode<ProgressBar>("Panel/Klimabelastung/KlimabelastungBar").Value = _klimabelastung;
        GetNode<ProgressBar>("Panel/Wohlstand/WohlstandBar").Value = _wohlstand;
        GetNode<ProgressBar>("Panel/Arbeitslosigkeit/ArbeitslosigkeitBar").Value = _arbeitslosigkeit;


        BerechneWaehlerzustimmung();
    }

    private void BerechneWaehlerzustimmung()
    {
        if (_arbeitslosigkeit > Schwellenwert)
            _waehlerzustimmung -= (_arbeitslosigkeit - Schwellenwert) * 0.33;
        if (_klimabelastung > Schwellenwert)
            _waehlerzustimmung -= (_klimabelastung - Schwellenwert) * 0.33;
        if (_wohlstand < Schwellenwert)
            _waehlerzustimmung -= (_wohlstand - Schwellenwert) * -0.33;


        _waehlerzustimmung = Clamp0_100(_waehlerzustimmung);
        GetNode<ProgressBar>("Waehlerzustimmung").Value = _waehlerzustimmung;

        GD.Print(
            $"wohlstand: {_wohlstand} \n arbeitslosigkeit: {_arbeitslosigkeit}\n  klimabelastung: {_klimabelastung} \n waehlerschaft: {_waehlerzustimmung}");
    }

    private static double Clamp0_100(double wert) => Math.Clamp(wert, 0, 100);
}