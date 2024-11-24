import logo from './logo.svg';
import './App.css';
import Example from './components/Example';

function App() {
  return (
    <div className="bg-blue-500 text-white text-center py-10">
    <h1 className="text-4xl font-bold">Hello, Tailwind CSS!</h1>
    <p className="mt-4 text-lg">การตั้งค่า Tailwind CSS สำเร็จแล้ว 🎉</p>
    <Example></Example>
  </div>
  );
}

export default App;
