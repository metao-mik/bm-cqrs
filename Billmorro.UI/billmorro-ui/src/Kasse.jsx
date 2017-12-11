import React from 'react';
import dotnetify from 'dotnetify';
import './kasse.css';

dotnetify.hubServerUrl = "http://localhost:5000";

const MODE_EAN_oder_PREIS = 0;
const MODE_PREIS = 2;

class Kasse extends React.Component {
    constructor(props) {
        super(props);
        this.vm = dotnetify.react.connect("KasseVM", this);
        this.state = {
            mode:MODE_EAN_oder_PREIS,
            ean_preis_euro_menge:"",
            preis_cent:"",
            AktuellerBon:null,
            Steuersatz1:"",
            Steuersatz2:"",
            Steuersatz1Name:"",
            Steuersatz2Name:"",
            NettoBetrag:"",
            BruttoBetrag:"",
            PositionenZahl:""
         };
    }

    taste(code){
        switch (this.state.mode){
            default:break;
            case MODE_EAN_oder_PREIS:
                if (code===","){
                    if (this.state.ean_preis_euro_menge==="") this.setState({ean_preis_euro_menge:"0"});
                    this.setState({mode:MODE_PREIS});
                }
                else {
                    this.setState({ean_preis_euro_menge:this.state.ean_preis_euro_menge+code});
                }
                break;
            case MODE_PREIS:
                if (code===",") break;
                if (this.state.preis_cent.length===2) break;
                this.setState({preis_cent:this.state.preis_cent+code});
                break;
        }
    }

    taste_bksp(){
      if (this.state.mode===MODE_PREIS){
        if (this.state.preis_cent.length===0) {
          this.setState({mode:MODE_EAN_oder_PREIS});
        } else {
          const preis = this.state.preis_cent.substr(0,this.state.preis_cent.length-1)
          this.setState({preis_cent:preis});
        }
      } else {
        if (this.state.ean_preis_euro_menge.length>0){
          const preis = this.state.ean_preis_euro_menge.substr(0,this.state.ean_preis_euro_menge.length-1)
          this.setState({ean_preis_euro_menge:preis});
        }
      }
    }

    artikelClick(id){
      const menge = this.state.ean_preis_euro_menge;
      this.vm.$dispatch({Artikel_hinzufuegen:id});
      this.resetKeypad();
    }

    taste_ean(){
      this.vm.$dispatch({Barcode_hinzufuegen:this.state.ean_preis_euro_menge});
      this.resetKeypad();
    }
    taste_bar(){}
    taste_pos(){}

    resetKeypad(){
      this.setState({ean_preis_euro_menge:"",preis_cent:"",mode:MODE_EAN_oder_PREIS});
    }

    render_display(){
        if (this.state.mode===MODE_EAN_oder_PREIS){
            if (this.state.ean_preis_euro_menge ==="") return "|";
            return this.state.ean_preis_euro_menge;
        } else {
            return this.state.ean_preis_euro_menge+","+this.state.preis_cent;
        }
    }

    render_taste(ziffer){
        return <input type="button" key={"TASTE_"+ziffer} className="taste_zahl" value={ziffer} onClick={()=>this.taste(ziffer)}></input>;
    }

    render_position(pos){
        return ([
          <tr key={"TR-A-"+pos.Id}><td className="text" colSpan="4" >{pos.Bezeichnung}</td></tr>,
          <tr key={"TR-B-"+pos.Id}><td></td><td>{pos.Menge}</td><td>{pos.Steuersatz}</td><td>{pos.Positionspreis}</td></tr>
        ]);
    }

    render_artikel(art){
      return ([
        <tr key={"TR-A-"+art.Id} onClick={()=>this.artikelClick(art.Id)}><td className="text" colSpan="2" >{art.Bezeichnung}</td></tr>,
        <tr key={"TR-B-"+art.Id}  onClick={()=>this.artikelClick(art.Id)}><td></td><td>{art.EinzelpreisText}</td></tr>
      ]);
  }

    render_artikelliste(){
      if (!this.state.Artikelliste){
        return <div className="artikelliste">- Artikelliste wird geladen -</div>;
      }
      return <div className="artikelliste">
                  <table>
                      <tbody>
                          {this.state.Artikelliste.map(a=>this.render_artikel(a))}
                      </tbody>
                      </table>
      </div>;
    }

    render_bon(){
        if (this.state.AktuellerBon===null) return <div className="bon">"Kein Bon"</div>;
        return (
            <div className="bon">
                <div className="bonpositionen">
                  <table>
                      <tbody>
                          {this.state.Positionen.map(p=>this.render_position(p))}
                      </tbody>
                      </table>
                </div>
                <div className="bonfuss">
                <table>
                    <tbody>
                        <tr><td className="label">Netto</td><td>{this.state.NettoBetrag}</td></tr>
                        <tr><td className="label">MwSt {this.state.Steuersatz1Name}</td><td>{this.state.Steuersatz1}</td></tr>
                        <tr><td className="label">MwSt {this.state.Steuersatz2Name}</td><td>{this.state.Steuersatz2}</td></tr>
                        <tr><td className="label">Summe</td><td>{this.state.BruttoBetrag}</td></tr>
                        <tr><td className="label">Positionen</td><td>{this.state.PositionenZahl}</td></tr>
                    </tbody>
                </table>
                </div>
            </div>
        );
    }

    render() {
        return (
            <div className="verkauf">
            {this.render_bon()}
            <div className="eingabe">
            <div className="anzeige"><span>{this.render_display()}</span></div>
            <div className="tastenfeld">
            {["1","2","3","4","5","6","7","8","9",",","0"].map(x=>this.render_taste(x))}
            <input type="button" className="taste_zahl" value="<-"  onClick={()=>this.taste_bksp()}></input>
            <input disabled={this.state.mode===MODE_PREIS || this.state.ean_preis_euro_menge.length<5}  type="button" className="taste_zahl" value="EAN"  onClick={()=>this.taste_ean()}></input>
            <input type="button" className="taste_zahl" value="BAR"  onClick={()=>this.taste_bar()}></input>
            <input type="button" className="taste_zahl" value="POS"  onClick={()=>this.taste_pos()}></input>
            </div>
            </div>
            {this.render_artikelliste()}
            </div>
        );
    }
}

export default Kasse;