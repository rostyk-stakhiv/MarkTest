import React, { Component } from 'react';
import axios  from 'axios'
export class EnrollTest extends Component {
  constructor(props) {
    super(props);
    }

  componentDidMount(){
    axios.post(`/tests/${this.props.match.params.id}/enroll`,{},{ headers: {"Authorization" : `Bearer ${localStorage["token"]}`}})
  .then(()=>{
    var enroll = document.getElementById("enrollSucces");
    enroll.innerText=`Вас успішно зараховано на тест :)`
    setTimeout(()=>{window.location.href="/cabinet"},10000);
  })

  }
  render () {
    return (
      <div className="cabinet">
       <h1 className="Warning" id="enrollSucces"></h1>
       
       
       </div>
    );
  }
}