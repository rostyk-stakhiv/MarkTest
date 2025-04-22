import axios from 'axios';
import React, { Component } from 'react';
export class CheckResults extends Component {
  static displayName = CheckResults.name;

  constructor(props) {
    super(props);
    this.state = {
      tests:"",
      testname: "",
      fromDate:"",
      toDate: "",
    }
  }
  setName(n)
  {
      this.setState({name:n})
  }
  setStartDate(n)
  {
      this.setState({fromDate:n})
  }
  setEndDate(n)
  {
      this.setState({toDate:n})
  }
  setName(n)
  {
      this.setState({name:n})
  }
  FindTests(e)
  {
    e.preventDefault();
    axios.post("/tests/checkTests",
      {
        "name": this.state.name,
        "fromDate":this.state.fromDate,
        "toDate":this.state.toDate,
      }, { headers: { "Authorization": `Bearer ${localStorage["token"]}` } })
      .then((response) => { 
          this.setState({tests:response.data.tests})
          var results = document.getElementById("results");
          this.state.tests.forEach(element=>{
            var h2 = document.createElement("h2");
            if(element.user.surname===null||element.user.name===null)
            {
                h2.innerText = element.user.email+": "+element.mark
            }
            else{
            h2.innerText = element.user.surname + " "+element.user.name+": "+element.mark;
            }
            results.append(h2);

          })
      }).catch(error => alert(error))
  }
  setDifficulty(d)
  {
    this.setState({difficulty:d})
  }
  startTest(e)
  {
      e.preventDefault();
    axios.get(`/Tests/${this.props.match.params.id}/taketest?difficulty=${this.state.difficulty}`, { headers: { "Authorization": `Bearer ${localStorage["token"]}` } })
    .then(response => {
      console.log(response); 
      })
    .catch(error => alert(error));
  }
  componentDidMount() {
this.getNames();
  }
getNames() {
    var name = document.getElementById("name");
  name.innerHTML="";
  axios.get(`/tests/getNames`,{ headers: {"Authorization" : `Bearer ${localStorage["token"]}`}})
  .then(response=>{
    var names = response.data.testsNames;
    names.forEach(element => {
      var option = document.createElement("option");
      option.value = element;
      option.innerText= element;
      name.append(option)
    });
})
}

  render() {
    return (
      <div className="Test">
           <div className="InputField">
        <h3>Назва тесту: </h3>
          <select id="name" value={this.state.name} onChange={e => this.setName(e.target.value)}>
          </select>
          </div>
        <div className="InputField">
          <h3>Від: </h3>
          <input type="datetime-local" id="startdate" name="startdate" value ={this.state.startDate} onChange={e=> this.setStartDate(e.target.value)}/>
        </div>

        <div className="InputField">
          <h3>До: </h3>
          <input type="datetime-local" id="enddate" name="enddate" value ={this.state.endDate} onChange={e=> this.setEndDate(e.target.value)}/>
        </div> 
        <div className="InputField">
            <button className='StartTestBtn' onClick={e=>this.FindTests(e)}>Знайти тести</button>  
            </div>    
            <div id="results">
            
        </div> 
        </div>  
        
    );
  }
}