import { Link } from 'react-router-dom';

export default function Navbar() {
  return (
    <nav style={{ background: '#f8f9fa', padding: '10px 20px', display: 'flex', gap: 20 }}>
      <Link to="/projects">Project</Link>
      <Link to="/employees">Employees</Link>
      <Link to="/wizard/step1">New project (wizard)</Link>
    </nav>
  );
}