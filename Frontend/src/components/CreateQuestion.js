import axios from 'axios';
import React, { Component } from 'react';

export class CreateQuestion extends Component {
  static displayName = CreateQuestion.name;

  constructor(props) {
    super(props);
    this.state = {
      name: "",
      description: "",
      type: "1",
      mark: 0,
      theme: "",
      subject: "",
      answers: [],
      suggestedanswers: [],
      privacy: false,
      difficultylevel: 1,
      questionnumber:0
    }
  }
  setName(n) {
    if (n.length < 150) {
      this.setState({ name: n })
    }
    else {
      alert("Name can't contain more than 150 symbols")
    }
  }
  setTheme(n) {
    if (n.length < 150) {
      this.setState({ theme: n })
    }
    else {
      alert("Theme can't contain more than 150 symbols")
    }
  }
  setDifficulty(n) {
    this.setState({ difficultylevel: n })
  }
  setSubject(n) {
    if (n.length < 150) {
      this.setState({ subject: n })
    }
    else {
      alert("Subject can't contain more than 150 symbols")
    }
  }

  setDescription(d) {
    if (d.length < 1000) {
      this.setState({ description: d })
    }
    else {
      alert("Description can't contain more than 1000 symbols")
    }
  }
  setMark(p) {
    if (p >= 0) {
      this.setState({ mark: p })
    }
    else {
      alert("Mark must be greater than 0")
    }
  }
  AddQuestion(e)
  {
    e.preventDefault()
    var doc = document.getElementById("Answers")
    var question = document.createElement("input");
    console.log(doc)
    console.log(question)
    
    question.setAttribute("name","answer")
    if(this.state.type=="1")
    {
      question.setAttribute("type","radio")
      question.setAttribute("class","answer")
    }
    else if(this.state.type=="2")
    {
      question.setAttribute("type","checkbox")
      question.setAttribute("class","answer")
    }
    else
    {
      question.setAttribute("type","text")
      question.setAttribute("class","answerInput")
    }
    question.setAttribute("id","question"+this.state.questionnumber) 
    this.setState({questionnumber:this.state.questionnumber+1});
    question.setAttribute("value",document.getElementById("questionText").value)
    var br = document.createElement("br");
    if(this.state.type!=="3")
    {
    var label = document.createElement("label")
    label.setAttribute("for","question"+this.state.questionnumber)
    var h3 = document.createElement("h3");
    h3.innerText = document.getElementById("questionText").value
    
    label.append(h3)
    doc.append(question, label,br)
    }
    else{
      doc.append(question,br)
    }
  }

  setType(t) {
    this.setState({ type: t })

  }
  setPrivacy(n) {

    this.setState({ privacy: n.checked })
  }

  Add(e) {
    e.preventDefault();

    for(var i=0;i<this.state.questionnumber;i++)
    {
      var elem = document.getElementById(`question${i}`);
      if(this.state.type!=="3")
      {
      this.state.suggestedanswers.push(elem.value)
      }
      if(elem.checked||this.state.type==="3")
      {
        this.state.answers.push(elem.value)
      }
    }

    axios.post("/questions",
      {
        "ownerId": parseInt(localStorage["UserID"]),
        "name": this.state.name,
        "description": this.state.description,
        "type": parseInt(this.state.type),
        "mark": parseFloat(this.state.mark),
        "theme": this.state.theme,
        "subject": this.state.subject,
        "privacy": this.state.privacy,
        "difficultyLevel": this.state.difficultylevel,
        "answers":this.state.answers,
        "suggestedAnswers":this.state.suggestedanswers
      }, { headers: { "Authorization": `Bearer ${localStorage["token"]}` } }).then(() => { document.location.href = "/" }).catch(error => alert(error.response.data))
  }
  render() {
    return (
      <div>
        <div className="InputField">
          <h3>Назва: </h3>
          <input type="text" id="name" value={this.state.name} onChange={e => this.setName(e.target.value)} placeholder="Назва" />
        </div>

        <div className="InputField">
          <h3>Предмет: </h3>
          <input id="description" value={this.state.subject} onChange={e => this.setSubject(e.target.value)} placeholder="Предмет" />
        </div>

        <div className="InputField">
          <h3>Тема: </h3>
          <input type="text" id="name" value={this.state.theme} onChange={e => this.setTheme(e.target.value)} placeholder="Тема" />
        </div>

        <div className="InputField">
          <h3>Умова: </h3>
          <textarea id="description" value={this.state.description} onChange={e => this.setDescription(e.target.value)} placeholder="Умова" />
        </div>
        <div className="InputField">
          <h3>Тип: </h3>
          <select id="type" value={this.state.type} onChange={e => this.setType(e.target.value)}>
            <option value="1">Одна відповідь</option>
            <option value="2">Декілька відповідей</option>
            <option value="3">Вписати відповідь</option>
          </select>
        </div>

        <div className="InputField"><h3>Оцінка: </h3>
          <input type="number" step="0.01" max="1" min="0" id="mark" value={this.state.mark} onChange={e => this.setMark(e.target.value)} placeholder="Оцінка" /></div>

        

        <div className="InputField"><h3>Рівень складності: </h3>
          <input type="number" max="3" min="1" id="mark" value={this.state.difficultylevel} onChange={e => this.setDifficulty(e.target.value)} placeholder="Рівень складності" />
        </div>
        <div className="AnswersContainer"><h3>Відповіді: </h3>
        <textarea id = "questionText"></textarea>
        <button className="AddAnswerBtn" onClick={e=>this.AddQuestion(e)}>Додати</button>
        <div id="Answers">

          </div>
        </div>
        
        <div className="InputField">
          <input value={this.state.privacy} type="checkbox" id="privacy" onChange={e => this.setPrivacy(e.target)} />
          <label for="privacy"> <h3> Приватне</h3></label>
        </div>
        <button className="AddQuestionBtn" onClick={e => { this.Add(e) }}>Додати питання</button>
      </div>
    );
  }
}
