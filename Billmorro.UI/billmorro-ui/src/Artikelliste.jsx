import React from 'react';
import dotnetify from 'dotnetify';

dotnetify.hubServerUrl = "http://localhost:5000";

class Artikelliste extends React.Component {
  constructor(props,context) {
    super(props,context);
    this.state = { Artikelliste : null };
    this.vm = dotnetify.react.connect("KasseVM.ArtikellisteVM", this);
  }

  artikelClick(id){
    this.vm.$dispatch({Artikel:id});
  }

  render_artikel(art){
    return ([
      <tr key={"TR-A-"+art.Id} onClick={()=>this.artikelClick(art.Id)}><td className="text" colSpan="2" >{art.Bezeichnung}</td></tr>,
      <tr key={"TR-B-"+art.Id}  onClick={()=>this.artikelClick(art.Id)}><td></td><td>{art.EinzelpreisText}</td></tr>
    ]);
  }

  render () {
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
}

export default Artikelliste;