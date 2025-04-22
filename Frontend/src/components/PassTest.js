import axios from 'axios';
import React, { Component } from 'react';
export class PassTest extends Component {
  static displayName = PassTest.name;

  constructor(props) {
    super(props);
    this.state = {test:[],
      currentPosition:0,
      questionCount:0,
       timeRemaining:0
    }
  }
  checkTime()
  {
    this.setState({timeRemaining:this.state.timeRemaining-1})
    if(this.state.timeRemaining<1)
    {
      this.finishTest()
    }
  }
  next(){
    var index = this.state.currentPosition +1;
    console.log(index)
    console.log(this.state.questionCount-2)
    this.AddAnswers(this.state.test.questions[index],index)
  }
  previous()
  {
    var index = this.state.currentPosition -1;
    this.AddAnswers(this.state.test.questions[index],index)
  }
  componentWillUnmount()
  {
    this.finishTest()

  }
  finishTest()
  {
    axios.post(`/Tests/passtest`,this.state.test, { headers: { "Authorization": `Bearer ${localStorage["token"]}` } }).then((response)=>alert("Ваша оцінка: "+(Math.round(response.data.mark * 100) / 100)))
    setTimeout(()=>{
      window.location.href="/cabinet";
    },10000)
  }
  setAnswer(e)
  {
    
      var number = parseInt(e.target.id)
      if(this.state.test.questions[number].type==1)
      {
        this.state.test.questions[number].answers=[e.target.value]
      }
      else if(this.state.test.questions[number].type==2)
      {
        if(e.target.checked)
        {
        this.state.test.questions[number].answers.push(e.target.value)
        }
        else{
          this.state.test.questions[number].answers.splice(this.state.test.questions[number].answers.indexOf(e.target.value),1)
        }
      }
      else{
        if(e.target.value!="")
        {
        this.state.test.questions[number].answers=[e.target.value]
        }
        else{
          this.state.test.questions[number].answers=[]
        }
      }
      var colorChange = document.getElementById("act"+number)
      colorChange.setAttribute("class","nonactive")
      if(this.state.test.questions[number].answers[0]!==undefined)
      {
        console.log("here")
        colorChange.setAttribute("class","active")
      }
      else{
        colorChange.class = "nonactive"
      }
  }
  AddAnswers(q, num)
  {
    this.setState({currentPosition:num})
    var doc = document.getElementById("Answers")
    doc.innerHTML=""
    var h3 = document.createElement("h3")
    var br = document.createElement("br");
    h3.innerText=q.description 
    doc.append(h3,br)
    if(q.type===3)
    {

      var question = document.createElement("input");
      question.setAttribute("id",num) 
      question.setAttribute("class","answerInput")
      if(q.answers!==undefined||q.answers.length>1)
    {
      if(q.answers[0]!==undefined)
      {
        question.value = q.answers[0]
      }
      
    }
    question.onchange = e=>{this.setAnswer(e)}
    doc.append(question)
  }
    else{
    q.suggestedAnswers.forEach(element => {
    
    var question = document.createElement("input");
    question.setAttribute("class","answer")
    question.setAttribute("name","answer")
    question.setAttribute("id",num) 
    question.onchange = e=>{this.setAnswer(e)}
    
    var label = document.createElement("label")
    label.setAttribute("for",q.Id)
    var h3 = document.createElement("h3");
    h3.innerText = element
    var br = document.createElement("br");
    
    if(q.type==1)
    {
      
      question.setAttribute("type","radio")
    }
    else if(q.type==2)
    {
      question.setAttribute("type","checkbox")
      
    }
    question.setAttribute("value",element)
    if(q.answers!==undefined)
    {
    if(q.answers.includes(element)){
      question.checked=true;
    }
  }
    label.append(h3)
    doc.append(question, label,br)
  });
}
  }

  componentDidMount() {
      const query = new URLSearchParams(this.props.location.search);
    axios.get(`/Tests/${this.props.match.params.id}/taketest?difficulty=${query.get('difficulty')}`, { headers: { "Authorization": `Bearer ${localStorage["token"]}` } })
    .then(response => {
      this.setState({test:response.data})
      this.AddAnswers(this.state.test.questions[0],0)
      this.setState({questionCount:this.state.test.questions.length})
      var navContainer=document.getElementById('NavigationContainer')
      if((this.state.test.endDate - Date.now)/1000<this.state.test.durability*60)
      {
        this.setState({timeRemaining:(this.state.test.endDate - Date.now)/1000})
      }
      else{
        this.setState({timeRemaining:this.state.test.durability*60})
      }
      for (let index = 0; index < this.state.test.questions.length; index++) {
        var questionNavigation = document.createElement("div");
        questionNavigation.setAttribute("class", "QuestionNav");
        questionNavigation.setAttribute("id", "nav"+index);
        questionNavigation.onclick= e=>{e.preventDefault();this.AddAnswers(this.state.test.questions[index],index)}
        var colorChange = document.createElement("div");
        colorChange.setAttribute("class", "colorChange");
        colorChange.setAttribute("id", "act"+index)
        var p = document.createElement("p");
        p.innerText = index+1;
        questionNavigation.append(p,colorChange)
        navContainer.append(questionNavigation)
      }
    setInterval(()=>{this.checkTime()},1000)
    })
    .catch(error => {console.log(error.data);alert(error.data)});
    
  }

  render() {
    return (
      <div className="TestContainer">
        <div></div><h3>Залишилось часу: {parseInt(this.state.timeRemaining/60)}:{this.state.timeRemaining%60}</h3>
<div className='QuestionsNavigation' id='NavigationContainer'>
</div>
          <div className='AnswersView' id="Answers">
        <h3>{this.state.description}</h3>
        <br></br>

        </div>
        <div></div>
        <div>{this.state.currentPosition>0&&<button class="TestNavBtn" onClick={e=>{e.preventDefault();this.previous()}}>Назад</button>}
        {this.state.currentPosition<(this.state.questionCount-1)&&<button class="TestNavBtn" onClick={e=>{e.preventDefault();this.next()}}>Далі</button>}
        {this.state.currentPosition===(this.state.questionCount-1)&&<button class="TestNavBtn" onClick={e=>{e.preventDefault();this.finishTest()}}>Завершити</button>}
        </div>
        
      </div>
    );
  }
}