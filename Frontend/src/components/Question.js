import axios from 'axios';
import React, { Component } from 'react';
export class Question extends Component {
  static displayName = Question.name;

  constructor(props) {
    super(props);
    this.state = {
      question:"",
      name: "",
      isOwner:false,
      description: "",
      type: "1",
      suggestedanswers: [],
      questionnumber:0
    }
  }
  componentDidMount() {
    axios.get(`/Questions/${this.props.match.params.id}`, { headers: { "Authorization": `Bearer ${localStorage["token"]}` } })
      .then(response => {
        console.log(response); this.setState({
          question:response.data,
          name: response.data.name,
          description: response.data.description,
          type: response.data.type,
          suggestedanswers: response.data.suggestedAnswers,
          isOwner:response.data.ownerId===parseInt(localStorage["UserID"])
        })
      }).then(()=>{
        this.AddAnswers()
      }).catch(error => alert(error));
  }

  AddAnswers(question)
  {
    var doc = document.getElementById("Answers")
    if(this.state.type===3)
    {
      var question = document.createElement("input");
      question.setAttribute("class","answerInput")
      doc.append(question)
    }
    else{
    this.state.suggestedanswers.forEach(element => {
      console.log(element)
    
    var question = document.createElement("input");
    question.setAttribute("class","answer")
    question.setAttribute("name","answer")
    question.setAttribute("id","question"+this.state.questionnumber) 
    this.setState({questionnumber:this.state.questionnumber+1});
    
    var label = document.createElement("label")
    label.setAttribute("for","question"+this.state.questionnumber)
    var h3 = document.createElement("h3");
    h3.innerText = element
    var br = document.createElement("br");
    
    if(this.state.type==1)
    {
      question.setAttribute("type","radio")
    }
    else if(this.state.type==2)
    {
      question.setAttribute("type","checkbox")
      
    }
    question.setAttribute("value",element)
    label.append(h3)
    doc.append(question, label,br)
  });
}
  }
  Remove(e)
  {
    e.preventDefault()
    axios.delete(`/Questions/${this.props.match.params.id}`,
     { headers: { "Authorization": `Bearer ${localStorage["token"]}` } })
     .then(() => { document.location.href = "/" }).catch(error => { alert(error.response.data) })
  }
  Copy(e)
  {
    e.preventDefault();
    axios.post("/questions/copy",
      {
        "id":this.state.question.id,
        "ownerId": parseInt(localStorage["UserID"]),
        "name": this.state.question.name,
        "description": this.state.question.description,
        "type": this.state.question.type,
        "mark": this.state.question.mark,
        "theme": this.state.question.theme,
        "subject": this.state.question.subject,
        "privacy": this.state.question.privacy,
        "difficultyLevel": this.state.question.difficultylevel,
        "answers":this.state.question.answers,
        "suggestedAnswers":this.state.question.suggestedanswers
      }, { headers: { "Authorization": `Bearer ${localStorage["token"]}` } }).then(() => { document.location.href = "/" }).catch(error => alert(error.response.data))
  }
  render() {
    return (
      <div className="Question">
        <h1>{this.state.name}</h1>

        <div className='QuestionView'>
        

        <div className='AnswersView' id="Answers">
        <h3>{this.state.description}</h3>
        <br></br>

        </div>
              {(this.state.isOwner||localStorage["isAdmin"]==="true")&&<button className='DeleteButton' onClick={e=>{this.Remove(e)}}>Видалити питання</button>}
              </div>

              {!(this.state.isOwner)&&<button className='CopyButton' onClick={e=>{this.Copy(e)}}>Скопіювати питання</button>}
              </div>   
    );
  }
}