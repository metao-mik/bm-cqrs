import React from 'react';
import dotnetify from 'dotnetify';

dotnetify.hubServerUrl = "http://localhost:5000";

class Touchpad extends React.Component {
  constructor(props,context) {
    super(props,context);
    this.state={
      DisplayText : "Verbinde..."
    };
    this.vm = dotnetify.react.connect("KasseVM.TouchpadVM", this);
  }

  taste (code) {
    this.vm.$dispatch({Taste:code});
  }
  taste_bksp() {
    this.vm.$dispatch({Taste_Backspace:null});
  }
  taste_barcode () {
    this.vm.$dispatch({Taste_Barcode:null});
  }
  taste_bar () {
    this.vm.$dispatch({Taste_KassierenBar:null});
  }
  taste_pos () {
    this.vm.$dispatch({Taste_Position:null});
  }

  render_taste(ziffer){
    return <input type="button" disabled={!this.state.EingabeMoeglich} key={"TASTE_"+ziffer} className="taste_zahl" value={ziffer} onClick={()=>this.taste(ziffer)}></input>;
  }

  render(){
    return (
      <div className="eingabe">
        <div className="anzeige"><span>{this.state.DisplayText}</span></div>
        <div className="tastenfeld">
          {["1","2","3","4","5","6","7","8","9",",","0"].map(x=>this.render_taste(x))}
        <input type="button" disabled={!this.state.EingabeMoeglich} className="taste_zahl" value="<-"  onClick={()=>this.taste_bksp()}></input>
        <input type="button" disabled={!this.state.BarcodeMoeglich} className="taste_zahl" value="EAN"  onClick={()=>this.taste_barcode()}></input>
        <input type="button" disabled={!this.state.KassierenBarMoeglich} className="taste_zahl" value="BAR"  onClick={()=>this.taste_bar()}></input>
        <input type="button" disabled={!this.state.BarPreisMoeglich} className="taste_zahl" value="POS"  onClick={()=>this.taste_pos()}></input>
        </div>
      </div>
    );
  }
}



export default Touchpad;