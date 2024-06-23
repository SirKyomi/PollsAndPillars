extends Control

#Stats
var _wohlstand;
var _arbeitslosigkeit;
var _klimabelastung;
var _waehlerzustimmung;

const Schwellenwert = 50;

# Timer und so
var timer = Timer.new();
const wait_time = 0.05;
const sekunden_pro_wahlperiode = 60.0;

var arbeitslosen_timer = Timer.new();
const arbeitslosen_steigerung_pro_wahlperiode = 24;
var arbeitslosen_timer_wait_time = sekunden_pro_wahlperiode / arbeitslosen_steigerung_pro_wahlperiode;
const faktor_arbeitslosen_steigerung = 2;

var vergangene_zeit = 0;
# Called when the node enters the scene tree for the first time.
func _ready():
	setze_start_werte();
	initialisiere_timer();
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	handle_game_over();

func handle_game_over():
	if(_waehlerzustimmung <= 0): 
		get_tree().reload_current_scene();

func setze_start_werte():
	_wohlstand = 50;
	_arbeitslosigkeit = 50;
	_klimabelastung = 50;
	_waehlerzustimmung = 100;

func initialisiere_timer():
	timer.wait_time = wait_time;
	timer.autostart = true;
	timer.one_shot = false;
	timer.timeout.connect(timer_on_timeout);
	
	arbeitslosen_timer.wait_time = arbeitslosen_timer_wait_time;
	arbeitslosen_timer.autostart = true;
	arbeitslosen_timer.one_shot = false;
	arbeitslosen_timer.timeout.connect(erhoehe_arbeitslosigkeit);

	add_child(timer);
	add_child(arbeitslosen_timer);

func timer_on_timeout():
	vergangene_zeit += wait_time;
	var rest = fmod(vergangene_zeit,sekunden_pro_wahlperiode);
	var berechnete_prozent = (rest / sekunden_pro_wahlperiode) * 100;
	get_node("WahlperiodeBar").value = berechnete_prozent;

func erhoehe_arbeitslosigkeit():
	_arbeitslosigkeit += faktor_arbeitslosen_steigerung;
	arbeitslosigkeit_ui_aktualisieren();
	berechne_waehlerzustimmung();

func arbeitslosigkeit_ui_aktualisieren():
	_arbeitslosigkeit = clamp_0_100(_arbeitslosigkeit);
	get_node("Panel/Arbeitslosigkeit/ArbeitslosigkeitBar").value = _arbeitslosigkeit;

func wohlstand_ui_aktualisieren():
	_wohlstand = clamp_0_100(_wohlstand);
	get_node("Panel/Wohlstand/WohlstandBar").value = _wohlstand;

func klimabelastung_ui_aktualisieren():
	_klimabelastung = clamp_0_100(_klimabelastung);
	get_node("Panel/Klimabelastung/KlimabelastungBar").value = _klimabelastung;

func berechne_waehlerzustimmung():
	if (_arbeitslosigkeit > Schwellenwert):
		_waehlerzustimmung -= (_arbeitslosigkeit - Schwellenwert) * 0.33;
	if (_klimabelastung > Schwellenwert):
		_waehlerzustimmung -= (_klimabelastung - Schwellenwert) * 0.33;
	if (_wohlstand < Schwellenwert):
		_waehlerzustimmung -= (_wohlstand - Schwellenwert) * -0.33;
		
	waehlerzustimmung_ui_aktualisieren();

func waehlerzustimmung_ui_aktualisieren():
	_waehlerzustimmung = clamp_0_100(_waehlerzustimmung);
	get_node("Waehlerzustimmung").value = _waehlerzustimmung;

func wende_werte_an_und_berechne_waehlerzustimmung(wohlstand, arbeitslosigkeit, klimabelastung):
	if klimabelastung >= 0:
		_klimabelastung += klimabelastung;
	else:
		_klimabelastung -= klimabelastung * -1;
	
	if wohlstand >= 0:
		_wohlstand += wohlstand;
	else:
		_wohlstand -= wohlstand * -1;
	
	if arbeitslosigkeit >= 0:
		_arbeitslosigkeit += arbeitslosigkeit;
	else:
		_arbeitslosigkeit -= arbeitslosigkeit * -1;
	arbeitslosigkeit_ui_aktualisieren();
	wohlstand_ui_aktualisieren();
	klimabelastung_ui_aktualisieren();
	berechne_waehlerzustimmung();

func clamp_0_100(value):
	return clamp(value,0,100);
