# Simple Casino Game System

## Description
This project implements a lightweight, real-time casino game system. Players can create and join tables, play various casino games, and receive payouts automatically. All player communications are handled via SignalR for instant interactions. Designed for developers and enthusiasts interested in real-time web applications, microservices, and gaming systems.

## Tech Stack
- ASP.NET Core with SignalR for real-time communication
- Microservices architecture for modular design
- JSON-based configuration for game rules
- JWT for mocked user authentication
- Frontend in React (planned implementation)
- Infrastructure tools for deployment

## Installation
This project includes multiple services. To set up locally:
1. Clone the repository:
```bash
git clone <repository-url>
```
2. Configure each service as per documentation (not included here for brevity).
3. Run services concurrently using Docker Compose or individual startup scripts.

## Usage
- Players connect via the frontend client (to be developed in React).
- Lobby handles matchmaking, game management, and payout logic.
- Other services (Auth, Wallet, Game services) run independently.

## Implementation Steps
- Develop Authentication Service with JWT user mocking.
- Implement Wallet Service to handle transactions.
- Build Lobby Service to manage game sessions and enforce security rules.
- Create individual game services (like Game1 Service) for game logic.
- Develop Frontend with React for users to interact in real-time.
- Arrange infrastructure for deployment, testing, and scaling.

**When you are done, submit the project from your profile:** https://bitasmbl.com/home/profile