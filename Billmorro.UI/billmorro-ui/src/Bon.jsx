import React from 'react';
import dotnetify from 'dotnetify';

dotnetify.hubServerUrl = "http://localhost:5000";


class Bon extends React.Component {
  constructor(props,context) {
    super(props,context);
    this.state = { AktuellerBon : null };
    this.vm = dotnetify.react.connect("KasseVM.BonVM", this);
  }

  render_position(pos){
    return ([
      <tr key={"TR-A-"+pos.Id}><td className="text" colSpan="4" >{pos.Bezeichnung}</td></tr>,
      <tr key={"TR-B-"+pos.Id}><td></td><td>{pos.Menge}</td><td>{pos.Steuersatz}</td><td>{pos.Positionspreis}</td></tr>
    ]);
  }

  render ()
  {
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
}


export default Bon;